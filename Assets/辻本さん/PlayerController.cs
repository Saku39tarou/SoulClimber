using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	public enum State
	{
		Normal,
		Ghost
	}

	[Header("基本移動設定")]
	[SerializeField] private float speed = 3f;
	[SerializeField] private float dashSpeed = 6f;
	[SerializeField] private float dashAcceleration = 10f;

	[Header("ジャンプ設定")]
	[SerializeField] private float jumpSpeed = 7f;

	[Header("重力設定")]
	[SerializeField] private float gravity = 15f;
	[SerializeField] private float fallSpeed = 10f;
	[SerializeField] private float initFallSpeed = 2f;

	[Header("カメラ設定")]
	[SerializeField] private Camera targetCamera;

	[Header("壁登り設定")]
	[SerializeField] private float climbSpeed = 3f;
	[SerializeField] private float wallCheckDistance = 0.6f;

	[Header("ゴースト設定")]
	[SerializeField] private float ghostSpeed = 5f;
	[SerializeField] private float ghostVerticalSpeed = 3f;

	private Transform _transform;
	private CharacterController characterController;
	private Vector2 inputMove;
	private float verticalVelocity;
	private float turnVelocity;
	private bool isGroundedPrev;
	private bool isClimbing;

	private bool isDashing;
	private float currentSpeed;

	private State currentState = State.Normal;

	// アニメーション
	private Animator animator;

	// 入力
	public void OnMove(InputAction.CallbackContext context)
	{

		inputMove = context.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (currentState == State.Ghost) return; // ゴースト中はジャンプ禁止
		if (!context.performed) return;
		if (!characterController.isGrounded) return;

		//if (!context.performed || !characterController.isGrounded) return;
		verticalVelocity = jumpSpeed;
		animator.SetTrigger("Jump");
		//Debug.Log("OnJump");
	}

	private void Awake()
	{
		_transform = transform;
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		if (targetCamera == null)
			targetCamera = Camera.main;

		currentSpeed = speed;
	}

	private void Update()
	{
		if (!gameObject.activeInHierarchy || !characterController.enabled)
			return;

		switch (currentState)
		{
			case State.Normal:
				UpdateNormal();
				break;

			case State.Ghost:
				UpdateGhost();
				break;
		}

		UpdateAnimator();
	}


	private void UpdateAnimator()
	{
		Vector3 horizontalVelocity = characterController.velocity;
		horizontalVelocity.y = 0f;

		// 移動量
		animator.SetFloat("Speed", horizontalVelocity.magnitude);

		// 接地
		animator.SetBool("IsGrounded", characterController.isGrounded);

		// 落下
		animator.SetFloat("yVelocity", verticalVelocity);

		// 壁登り
		animator.SetBool("IsClimbing", isClimbing);
	}
	// 通常
	private void UpdateNormal()
	{
		bool isGrounded = characterController.isGrounded;

		bool isTouchingWall = false;

		// 壁判定
		RaycastHit hit;
		float sphereRadius = 0.4f;

		// Rayを中心位置に
		Vector3 start = _transform.position + Vector3.up * (characterController.height * 0.3f);

		if (Physics.SphereCast(start, sphereRadius, _transform.forward, out hit, wallCheckDistance))
		{
			float wallDot = Vector3.Dot(hit.normal, Vector3.up);
			if (wallDot < 0.5f)
			{
				isTouchingWall = true;
			}
		}

		// 壁登り
		if (isTouchingWall && Input.GetMouseButton(0))
		{
			isClimbing = true;

			// 壁の法線方向を基に登る方向を計算
			Vector3 climbDir = Vector3.ProjectOnPlane(Vector3.up, -hit.normal).normalized;
		
			// 上昇・下降
			float inputVeertical = inputMove.y;
			Vector3 climbMove = climbDir * (inputVeertical * climbSpeed);

			//壁に押し付ける力
			climbMove += -hit.normal * 0.1f;

			characterController.Move(climbMove * Time.deltaTime);

			// キャラの向きを壁の方向に合わせる
			Quaternion targetRot = Quaternion.LookRotation(-hit.normal);
			_transform.rotation = Quaternion.Slerp(_transform.rotation, targetRot, Time.deltaTime * 10f);
			
			// 登りきり判定
			if(!Physics.Raycast(start, _transform.forward, out _, wallCheckDistance))
			{
				isClimbing = false;
			}
			return;
		}
		else
		{
			isClimbing = false;
		}

		// ダッシュ
		if (Keyboard.current.leftShiftKey.isPressed && !isClimbing && isGrounded)
		{
			isDashing = true;
		}
		else
		{
			isDashing = false;
		}
		Debug.Log(isDashing);

		float targetSpeed = isDashing ? dashSpeed : speed;
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * dashAcceleration);

		// 重力
		if (!isClimbing)
		{
			if (isGrounded && !isGroundedPrev)
				verticalVelocity = -initFallSpeed;
			else if (!isGrounded)
			{
				verticalVelocity -= gravity * Time.deltaTime;
				if (verticalVelocity < -fallSpeed)
					verticalVelocity = -fallSpeed;
			}
		}
		isGroundedPrev = isGrounded;

		// 移動
		float cameraAngleY = targetCamera.transform.eulerAngles.y;
		Vector3 moveVelocity = new Vector3(inputMove.x * currentSpeed, verticalVelocity, inputMove.y * currentSpeed);
		moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;
		characterController.Move(moveVelocity * Time.deltaTime);

		// 向き
		if (inputMove != Vector2.zero)
		{
			float targetAngleY = -Mathf.Atan2(inputMove.y, inputMove.x) * Mathf.Rad2Deg + 90;
			targetAngleY += cameraAngleY;
			float angleY = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngleY, ref turnVelocity, 0.1f);
			_transform.rotation = Quaternion.Euler(0, angleY, 0);
		}
	}

	// ゴースト
	private void UpdateGhost()
	{
		Vector3 move = new Vector3(inputMove.x, 0, inputMove.y);

		// カメラ基準で移動方向を変換
		float cameraY = targetCamera.transform.eulerAngles.y;
		move = Quaternion.Euler(0, cameraY, 0) * move;
		move.Normalize();

		// 上下移動
		float upDown = 0f;
		if (Keyboard.current.spaceKey.isPressed)
			upDown += 1f;
		if (Keyboard.current.leftCtrlKey.isPressed)
			upDown -= 1f;

		move.y = upDown * ghostVerticalSpeed;

		// 実際の移動
		characterController.Move(move * ghostSpeed * Time.deltaTime);
	}

	private bool CheckClimbOver(RaycastHit wallHit)
	{
		Vector3 headPos = _transform.position + Vector3.up * (characterController.height * 0.5f);
		if(Physics.Raycast(headPos, Vector3.up, out RaycastHit upHit, 0.8f))
		{
			return false;
		}

		Vector3 forwardPos = _transform.position + (-wallHit.normal * 0.3f) + Vector3.up * 1.0f;
		if(Physics.Raycast(forwardPos, Vector3.down, out RaycastHit groundHit, 1.5f))
		{
			if(Vector3.Dot(groundHit.normal, Vector3.up) > 0.8f)
			{
				return true;
			}
		}
		return false;
	}

	// 状態切り替え
	public void SetState(State newState)
	{
		currentState = newState;
		//Debug.Log($"State changed to: {currentState}");
	}

	public State GetState()
	{
		return currentState;
	}

	private void OnDrawGizmosSelected()
	{
		if (_transform == null) _transform = transform;
		if (characterController == null) characterController = GetComponent<CharacterController>();

		float sphereRadius = 0.5f;
		float distance = wallCheckDistance;

		Vector3 start = _transform.position + Vector3.up * (characterController.height * 0.5f);
		Vector3 end = start + _transform.forward * distance;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(start, sphereRadius); // 始点
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(end, sphereRadius);   // 終点
		Gizmos.color = Color.white;
		Gizmos.DrawLine(start, end);                // 線

		// 登りきりチェック位置のデバック
		Gizmos.color = Color.green;
		Vector3 headPos = _transform.position + Vector3.up * (characterController.height * 0.5f);
		Gizmos.DrawWireSphere(headPos, 0.1f);
	}
}
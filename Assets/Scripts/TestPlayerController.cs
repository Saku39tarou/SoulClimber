using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TestPlayerController : MonoBehaviour
{
	[Header("基本移動設定")]
	[SerializeField] private float speed = 3f;
	[SerializeField] private float dashSpeed = 6f;					// ダッシュ時の速さ
	[SerializeField] private float dashAcceleration = 10f;			// ダッシュへの切り替えの滑らかさ

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

	private Transform _transform;
	private CharacterController characterController;
	private Vector2 inputMove;
	private float verticalVelocity;
	private float turnVelocity;
	private bool isGroundedPrev;
	private bool isClimbing = false;

	private bool isDashing = false;
	private float currentSpeed;

	public void OnMove(InputAction.CallbackContext context)
	{
		// 入力値を保持しておく
		inputMove = context.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		// ボタンが押されている瞬間かつ着地している時だけ処理
		if (!context.performed || !characterController.isGrounded) return;

		// 鉛直上向きに速度を与える
		verticalVelocity = jumpSpeed;
	}

	private void Awake()
	{
		_transform = transform;
		characterController = GetComponent<CharacterController>();
		if (targetCamera == null)
			targetCamera = Camera.main;

		currentSpeed = speed;
	}

	private void Update()
	{
		bool isGrounded = characterController.isGrounded;

		// 壁判定
		bool isTouchingWall = Physics.Raycast(_transform.position, _transform.forward, wallCheckDistance);

		// 壁登り処理
		if (isTouchingWall && Input.GetMouseButton(0))
		{
			isClimbing = true;
			verticalVelocity = climbSpeed;
		}
		else
		{
			isClimbing = false;
		}

		// ダッシュ処理
		if (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed && !isClimbing && isGrounded)
		{
			isDashing = true;
		}
		else
		{
			isDashing = false;
		}

		float targetSpeed = isDashing ? dashSpeed : speed;
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * dashAcceleration);

		// 重力処理
		if (!isClimbing)
		{
			if (isGrounded && !isGroundedPrev)
			{
				verticalVelocity = -initFallSpeed;
			}
			else if (!isGrounded)
			{
				verticalVelocity -= gravity * Time.deltaTime;
				if (verticalVelocity < -fallSpeed)
					verticalVelocity = -fallSpeed;
			}
		}

		isGroundedPrev = isGrounded;

		// 移動処理
		float cameraAngleY = targetCamera.transform.eulerAngles.y;
		Vector3 moveVelocity = new Vector3(inputMove.x * currentSpeed, verticalVelocity, inputMove.y * currentSpeed);
		moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;

		Vector3 moveDelta = moveVelocity * Time.deltaTime;
		characterController.Move(moveDelta);

		// 向き処理
		if (inputMove != Vector2.zero)
		{
			float targetAngleY = -Mathf.Atan2(inputMove.y, inputMove.x) * Mathf.Rad2Deg + 90;
			targetAngleY += cameraAngleY;
			float angleY = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngleY, ref turnVelocity, 0.1f);
			_transform.rotation = Quaternion.Euler(0, angleY, 0);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (_transform != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(_transform.position, _transform.forward * wallCheckDistance);
		}
	}
}


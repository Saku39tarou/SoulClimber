using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TestPlayerController : MonoBehaviour
{
	[Header("基本移動設定")]
	[SerializeField] private float speed = 3f;
	[SerializeField] private float dashSpeed = 6f; // ダッシュ時の速さ
	[SerializeField] private float dashAcceleration = 10f; // ダッシュへの切り替えの滑らかさ

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

	// 入力 
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

/*
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TestPlayerController : MonoBehaviour
{
	[Header("移動の速さ"), SerializeField] private float speed = 3;
	[Header("ジャンプする瞬間の速さ"), SerializeField] private float jumpSpeed = 7;
	[Header("重力加速度"), SerializeField] private float gravity = 15;
	[Header("落下時の速さ制限（Infinityで無制限）"), SerializeField] private float fallSpeed = 10;
	[Header("落下の初速"), SerializeField] private float initFallSpeed = 2;
	[Header("カメラ"), SerializeField] private Camera targetCamera;

	[Header("壁登り設定")]
	[SerializeField] private float climbSpeed = 2f;       // 登る速さ
	[SerializeField] private float climbCheckDistance = 0.6f; // 壁検知距離
	[SerializeField] private LayerMask climbableLayer;     // 登れる壁のレイヤー

	private Transform _transform;
	private CharacterController _characterController;
	private Vector2 _inputMove;
	private float _verticalVelocity;
	private float _turnVelocity;
	private bool _isGroundedPrev;

	private bool _isClimbing = false;

	public void OnMove(InputAction.CallbackContext context)
	{
		// 入力値を保持しておく
		_inputMove = context.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (_isClimbing)
		{
			// 登っている最中にジャンプで離れる
			_isClimbing = false;
			_verticalVelocity = jumpSpeed;
			return;
		}

		// ボタンが押されている瞬間かつ着地している時だけ処理
		if (!context.performed || !_characterController.isGrounded) return;

		// 鉛直上向きに速度を与える
		_verticalVelocity = jumpSpeed;
	}

	private void Awake()
	{
		_transform = transform;
		_characterController = GetComponent<CharacterController>();

		if (targetCamera == null)
			targetCamera = Camera.main;
	}

	private void Update()
	{
		if (!_isClimbing)
		{
			HandleMovement();
			CheckForClimbStart();
		}
		else
		{
			HandleClimbing();
		}
	}

	private void HandleMovement()
	{
		bool isGrounded = _characterController.isGrounded;

		if (isGrounded && !_isGroundedPrev)
		{
			// 着地する瞬間に落下の初速を指定しておく
			_verticalVelocity = -initFallSpeed;
		}
		else if (!isGrounded)
		{
			// 空中にいるときは、下向きに重力加速度を与えて落下させる
			_verticalVelocity -= gravity * Time.deltaTime;

			// 落下する速さ以上にならないように補正
			if (_verticalVelocity < -fallSpeed)
				_verticalVelocity = -fallSpeed;
		}

		_isGroundedPrev = isGrounded;

		// カメラの向き（角度[deg]）取得
		float cameraAngleY = targetCamera.transform.eulerAngles.y;

		// 操作入力と鉛直方向速度から、現在速度を計算
		Vector3 moveVelocity = new Vector3(
			_inputMove.x * speed,
			_verticalVelocity,
			_inputMove.y * speed
		);
		moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;

		_characterController.Move(moveVelocity * Time.deltaTime);

		if (_inputMove != Vector2.zero)
		{
			float targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x) * Mathf.Rad2Deg + 90;
			targetAngleY += cameraAngleY;
			float angleY = Mathf.SmoothDampAngle(
				_transform.eulerAngles.y,
				targetAngleY,
				ref _turnVelocity,
				0.1f
			);
			_transform.rotation = Quaternion.Euler(0, angleY, 0);
		}
	}

	private void CheckForClimbStart()
	{
		// 前方にRayを飛ばして壁を検出
		if (Physics.Raycast(_transform.position + Vector3.up * 1f, _transform.forward, out RaycastHit hit, climbCheckDistance, climbableLayer))
		{
			// 壁にぶつかっていて、前入力があるなら登る
			if (_inputMove.y > 0.1f)
			{
				_isClimbing = true;
				_verticalVelocity = 0;
			}
		}
	}

	private void HandleClimbing()
	{
		// 上に移動
		Vector3 climbMove = Vector3.up * climbSpeed * Time.deltaTime;
		_characterController.Move(climbMove);

		// 壁から離れたら登り終了
		if (!Physics.Raycast(_transform.position + Vector3.up * 1f, _transform.forward, climbCheckDistance, climbableLayer))
		{
			_isClimbing = false;
		}

		// 上入力がない場合も登り中断
		if (_inputMove.y <= 0.1f)
		{
			_isClimbing = false;
		}
	}
}
*/

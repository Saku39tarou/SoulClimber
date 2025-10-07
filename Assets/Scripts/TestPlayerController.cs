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

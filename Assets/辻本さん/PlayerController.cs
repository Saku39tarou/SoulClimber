/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] GameObject climber;
	[SerializeField] float walkspeed;
	[SerializeField] float junpSpeed;
	[SerializeField] float gravity;
	[SerializeField] GameObject cam;

	[SerializeField] float climbSpeed = 3.0f;
	[SerializeField] float wallCheckDistance = 0.6f; 
	public enum State
	{
		Walk,
		Climb,
		Ghost,
	}

	[SerializeField] State state;
	Quaternion cameraRot, characterRot;
	float Xsensityvity = 3f, Ysensityvity = 3f;
	[SerializeField] bool isClimbing = false;
	[SerializeField] bool isGround = false;
	[SerializeField] bool isJumping = false;

	//変数の宣言(角度の制限用)
	float minX = -80f, maxX = 80f;

	private Vector3 moveDirection = Vector3.zero;
	private Transform _transform;

	Rigidbody rb;
	Collider climbCollider;
	Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
		_transform = transform;
		animator = climber.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
		cameraRot = cam.transform.localRotation;
		characterRot = transform.localRotation;
		state = State.Walk;
	}

    // Update is called once per frame
    void Update()
    {
		// 正面にRayを飛ばして壁か判定
		bool isTouchingWall = Physics.Raycast(_transform.position, _transform.forward, wallCheckDistance);

		float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
		float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

		cameraRot *= Quaternion.Euler(-yRot, 0, 0);
		characterRot *= Quaternion.Euler(0, xRot, 0);

		//Updateの中で作成した関数を呼ぶ
		cameraRot = ClampRotation(cameraRot);

		cam.transform.localRotation = cameraRot;
		transform.localRotation = characterRot;
		if(state == State.Walk && isGround)
		{
			float veloY = rb.velocity.y;
			// Wキー（前方移動）
			if (Input.GetKey(KeyCode.W))
			{
				rb.velocity = transform.forward * walkspeed;
				animator.SetBool("Walk", true);

			}
			// Sキー（後方移動）
			else if (Input.GetKey(KeyCode.S))
			{
				rb.velocity = -transform.forward * walkspeed;
			}
			// Dキー（右移動）
			else if (Input.GetKey(KeyCode.D))
			{
				rb.velocity = transform.right * walkspeed;
			}
			// Aキー（左移動）
			else if (Input.GetKey(KeyCode.A))
			{
				rb.velocity = -transform.right * walkspeed;
			}
			else
			{
				rb.velocity = Vector3.zero;
				animator.SetBool("Walk", false);
				
			}
			rb.velocity = new Vector3(rb.velocity.x, veloY, rb.velocity.z);
			if(!isJumping && Input.GetKeyDown(KeyCode.Space))
			{
				rb.velocity = Vector3.up * junpSpeed;
				isJumping = true;
			}
		}

		if(state == State.Ghost)
		{
			Ghost();
		}
		
		if (isClimbing && isTouchingWall && Input.GetMouseButton(0))
		{
			
			Debug.Log("入った");
			
			Climb();
		}
		else
		{
			animator.SetBool("ClimbUp", false);
			animator.SetBool("ClimbDown", false);
			
		}

	}


	//角度制限関数の作成
	public Quaternion ClampRotation(Quaternion q)
	{
		//q = x,y,z,w (x,y,zはベクトル（量と向き）：wはスカラー（座標とは無関係の量）)

		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;

		float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

		angleX = Mathf.Clamp(angleX, minX, maxX);

		q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

		return q;
	}

	private void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.CompareTag("ClimbWall"))
		{
			state = State.Climb;
			isClimbing = true;
			
			//climbCollider = other;
			
		}

		if (other.gameObject.CompareTag("Floor"))
		{
			isGround = true;
			isJumping = false;
		}
			
	}

	//private void OnTriggerExit(Collider other)
	//{
	//	if(other == climbCollider)
	//	{
	//		isClimbing = false;
			
	//	}
	//}

	/*private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Floor"))
		{
			isGround = true;
		}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.CompareTag("Floor"))
		{
			isGround = false;
		}

		if (collision.gameObject.CompareTag("ClimbWall"))
		{
			isClimbing = false;
			rb.useGravity = true;
			state = State.Walk;
		}
	}

	private void Climb()
	{
		isGround = false;
		rb.useGravity = false;
		float verticalInput = Input.GetAxis("Vertical");
		Vector3 climbDirection = new Vector3(0, verticalInput * climbSpeed, 0);
		rb.velocity = climbDirection;

		if (verticalInput == 0)
		{
			animator.SetFloat("StopClimb", 0.0f);
		}
		else if (verticalInput > 0)
		{
			animator.SetBool("ClimbUp", true);
			animator.SetBool("ClimbDown", false);
			animator.SetFloat("StopClimb", 1.0f);
		}
		else
		{
			animator.SetBool("ClimbDown", true);
			animator.SetBool("ClimbUp", false);
			animator.SetFloat("StopClimb", 1.0f);
		}
	}

	private void Ghost()
	{
		isGround = false;
		rb.useGravity = false;
		// Wキー（前方移動）
		if (Input.GetKey(KeyCode.W))
		{
			rb.velocity = transform.forward * walkspeed;
			animator.SetBool("Walk", true);

		}
		// Sキー（後方移動）
		else if (Input.GetKey(KeyCode.S))
		{
			rb.velocity = -transform.forward * walkspeed;
		}
		// Dキー（右移動）
		else if (Input.GetKey(KeyCode.D))
		{
			rb.velocity = transform.right * walkspeed;
		}
		// Aキー（左移動）
		else if (Input.GetKey(KeyCode.A))
		{
			rb.velocity = -transform.right * walkspeed;
		}
		else if(Input.GetKey(KeyCode.F))
		{
			rb.velocity = transform.up * walkspeed;
		}
		else if(Input.GetKey(KeyCode.C))
		{
			rb.velocity = -transform.up * walkspeed;
		}
		else
		{
			rb.velocity = Vector3.zero;
		}
	}
	
	public void SetState(State st)
	{
		state = st;
	}


	// Rayのデバッグ用
	private void OnDrawGizmosSelected()
	{
		if (_transform != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(_transform.position, _transform.forward * wallCheckDistance);
		}
	}
}
*/
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
	private bool isClimbing = false;

	private bool isDashing = false;
	private float currentSpeed;

	private State currentState = State.Normal;

	// アニメーション
	//private Animator animator;

	// 入力
	public void OnMove(InputAction.CallbackContext context)
	{

		inputMove = context.ReadValue<Vector2>();
		// アニメーション
		//animator.SetBool("Run", inputMove != Vector2.zero);
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (currentState == State.Ghost) return; // ゴースト中はジャンプ禁止
		if (!context.performed || !characterController.isGrounded) return;
		verticalVelocity = jumpSpeed;
	}

	private void Awake()
	{
		_transform = transform;
		characterController = GetComponent<CharacterController>();
		//animator = GetComponent<Animator>();
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
	}

	// 通常モード
	private void UpdateNormal()
	{
		bool isGrounded = characterController.isGrounded;

		// 壁判定
		bool isTouchingWall = Physics.Raycast(_transform.position, _transform.forward, wallCheckDistance);

		// 壁登り
		if (isTouchingWall && Input.GetMouseButton(0))
		{
			isClimbing = true;
			verticalVelocity = climbSpeed;
		}
		else
		{
			isClimbing = false;
		}

		// ダッシュ
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

	// ゴーストモード
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

	// 状態切り替え
	public void SetState(State newState)
	{
		currentState = newState;
		Debug.Log($"State changed to: {currentState}");
	}

	public State GetState()
	{
		return currentState;
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


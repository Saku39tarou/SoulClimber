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

	enum State
	{
		Walk,
		Climb
	}

	State state;
	Quaternion cameraRot, characterRot;
	float Xsensityvity = 3f, Ysensityvity = 3f;
	bool isClimbing = false;
	bool isGround = false;

	//変数の宣言(角度の制限用)
	float minX = -80f, maxX = 80f;

	private Vector3 moveDirection = Vector3.zero;

	Rigidbody rb;
	Collider climbCollider;
	Animator animator;
    // Start is called before the first frame update
    void Start()
    {
		animator = climber.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
		cameraRot = cam.transform.localRotation;
		characterRot = transform.localRotation;
		state = State.Walk;
	}

    // Update is called once per frame
    void Update()
    {
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

		}

		
		if (isClimbing && Input.GetMouseButton(0))
		{
			
			Debug.Log("入った");
			
			Climb();
		}
		else
		{
			state = State.Walk;
			animator.SetBool("ClimbUp", false);
			animator.SetBool("ClimbDown", false);
			rb.useGravity = true;
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
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other == climbCollider)
		{
			isClimbing = false;
			
		}
	}

	/*private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Floor"))
		{
			isGround = true;
		}
	}*/

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.CompareTag("Floor"))
		{
			isGround = false;
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
}

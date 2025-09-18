using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] GameObject climber;
	[SerializeField] float speed;
	[SerializeField] float junpSpeed;
	[SerializeField] float gravity;
	[SerializeField] GameObject cam;

	Quaternion cameraRot, characterRot;
	float Xsensityvity = 3f, Ysensityvity = 3f;
	bool move;
	bool cursorLock = true;

	//�ϐ��̐錾(�p�x�̐����p)
	float minX = -90f, maxX = 90f;

	private Vector3 moveDirection = Vector3.zero;

	Rigidbody rb;	
	Animator animator;
    // Start is called before the first frame update
    void Start()
    {
		animator = climber.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
		cameraRot = cam.transform.localRotation;
		characterRot = transform.localRotation;
	}

    // Update is called once per frame
    void Update()
    {
		float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
		float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

		cameraRot *= Quaternion.Euler(-yRot, 0, 0);
		characterRot *= Quaternion.Euler(0, xRot, 0);

		//Update�̒��ō쐬�����֐����Ă�
		cameraRot = ClampRotation(cameraRot);

		cam.transform.localRotation = cameraRot;
		transform.localRotation = characterRot;


		UpdateCursorLock();

		// W�L�[�i�O���ړ��j
		if (Input.GetKey(KeyCode.W))
		{
			rb.velocity = transform.forward * speed;
			animator.SetBool("Walk", true);
		}
		else
		{
			animator.SetBool("Walk", false);
		}

		// S�L�[�i����ړ��j
		if (Input.GetKey(KeyCode.S))
		{
			rb.velocity = -transform.forward * speed;
		}

		// D�L�[�i�E�ړ��j
		if (Input.GetKey(KeyCode.D))
		{
			rb.velocity = transform.right * speed;
		}

		// A�L�[�i���ړ��j
		if (Input.GetKey(KeyCode.A))
		{
			rb.velocity = -transform.right * speed;
		}
	}

	public void UpdateCursorLock()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			cursorLock = false;
		}
		else if (Input.GetMouseButton(0))
		{
			cursorLock = true;
		}


		if (cursorLock)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		else if (!cursorLock)
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	//�p�x�����֐��̍쐬
	public Quaternion ClampRotation(Quaternion q)
	{
		//q = x,y,z,w (x,y,z�̓x�N�g���i�ʂƌ����j�Fw�̓X�J���[�i���W�Ƃ͖��֌W�̗ʁj)

		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;

		float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

		angleX = Mathf.Clamp(angleX, minX, maxX);

		q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

		return q;
	}
}

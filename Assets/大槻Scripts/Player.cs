using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	Rigidbody rigid;
	public float speed = 10.0f;
	public float jump = 5.5f;
	private bool isjumpPower = false;//�ł���ł��Ȃ���false��true��




	// Start is called before the first frame update
	void Start()
	{
		this.rigid = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		//�O�i
		if (Input.GetKey(KeyCode.W))
		{
			transform.position += speed * transform.forward * Time.deltaTime;
		}
		//�E
		if (Input.GetKey(KeyCode.S))
		{
			transform.position -= speed * transform.forward * Time.deltaTime;
		}
		//��
		if (Input.GetKey(KeyCode.A))
		{
			transform.position -= speed * transform.right * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.D))
		{
			transform.position += speed * transform.right * Time.deltaTime;
		}

		//�W�����v
		if (Input.GetKeyDown(KeyCode.Space) && !isjumpPower)
		{
			rigid.velocity = Vector3.up * jump;//�W�����v���x
			isjumpPower = true;
		}

	}
	//2�i�W�����v�֎~�i�󒆂ŏo���Ȃ��悤�ɂ���j
	private void OnCollisionEnter(Collision collision)//�Փ˂�����E�E�E
	{
		if (collision.gameObject.CompareTag("Floor"))//Floor�^�O�̃I�u�W�F�N�g�ɐG�ꂽ�������x�W�����v�ł���悤�ɂȂ�
		{
			isjumpPower = false;
		}

	}
}


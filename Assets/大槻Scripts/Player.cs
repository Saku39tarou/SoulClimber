using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	Rigidbody rigid;
	public float speed = 10.0f;
	public float jump = 5.5f;
	private bool isjumpPower = false;//できるできないをfalseとtrueで




	// Start is called before the first frame update
	void Start()
	{
		this.rigid = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		//前進
		if (Input.GetKey(KeyCode.W))
		{
			transform.position += speed * transform.forward * Time.deltaTime;
		}
		//右
		if (Input.GetKey(KeyCode.S))
		{
			transform.position -= speed * transform.forward * Time.deltaTime;
		}
		//左
		if (Input.GetKey(KeyCode.A))
		{
			transform.position -= speed * transform.right * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.D))
		{
			transform.position += speed * transform.right * Time.deltaTime;
		}

		//ジャンプ
		if (Input.GetKeyDown(KeyCode.Space) && !isjumpPower)
		{
			rigid.velocity = Vector3.up * jump;//ジャンプ速度
			isjumpPower = true;
		}

	}
	//2段ジャンプ禁止（空中で出来ないようにする）
	private void OnCollisionEnter(Collision collision)//衝突したら・・・
	{
		if (collision.gameObject.CompareTag("Floor"))//Floorタグのオブジェクトに触れたらもう一度ジャンプできるようになる
		{
			isjumpPower = false;
		}

	}
}


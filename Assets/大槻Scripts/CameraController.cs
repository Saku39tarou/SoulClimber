using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	//public float mouseSensitivity = 0.5f;//感度
	public float mouseSensitivity = 250f;//感度
	private float xRotation = 0f;//x軸回転

	private void Start()
	{
		//Cursor.lockState = CursorLockMode.Locked;
	}
	private void Update()
	{
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);//カメラの回転速度
													  //xRotation = Mathf.Clamp(xRotation, -90f, 90f);//カメラの回転速度

		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		transform.parent.Rotate(Vector3.up * mouseX);
	}
}

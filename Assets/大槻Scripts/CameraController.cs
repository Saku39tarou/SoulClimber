using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	//public float mouseSensitivity = 0.5f;//���x
	public float mouseSensitivity = 250f;//���x
	private float xRotation = 0f;//x����]

	private void Start()
	{
		//Cursor.lockState = CursorLockMode.Locked;
	}
	private void Update()
	{
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);//�J�����̉�]���x
													  //xRotation = Mathf.Clamp(xRotation, -90f, 90f);//�J�����̉�]���x

		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		transform.parent.Rotate(Vector3.up * mouseX);
	}
}

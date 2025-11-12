/*
using UnityEngine;

public class CameraPOV : MonoBehaviour
{
	[Header("ターゲット設定")]
	private Transform playerTarget;		// プレイヤー
	private Transform ghostTarget;		// ゴースト
	private Transform currentTarget;	// 現在追っているターゲット

	[Header("回転設定")]
	public float sensitivity = 150f;
	public float minY = -80f;
	public float maxY = 80f;

	private float rotX;
	private float rotY;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		// 最初はPlayerを追従
		currentTarget = playerTarget;
	}

	void LateUpdate()
	{
		if (currentTarget == null) return;

		// マウス入力で回転
		float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

		rotY += mouseX;
		rotX -= mouseY;
		rotX = Mathf.Clamp(rotX, minY, maxY);

		// カメラの回転を適用
		transform.rotation = Quaternion.Euler(rotX, rotY, 0);

		// ターゲットの位置に追従
		transform.position = currentTarget.position;
	}

	// ターゲット切り替え関数
	public void SetTarget(Transform newTarget)
	{
		currentTarget = newTarget;
	}

	public void SwitchToPlayer() => currentTarget = playerTarget;
	public void SwitchToGhost() => currentTarget = ghostTarget;
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPOV : MonoBehaviour
{
	[SerializeField] GameObject cam;
	[SerializeField] InputActionReference lookAction;
	[SerializeField] float mouseSensitivity = 0.1f;
	[SerializeField] float gamepadSensitivity = 3.0f;

	[SerializeField] float minX = -90f;
	[SerializeField] float maxX = 90f;

	Quaternion cameraRot, characterRot;

	bool cursorLock = true;

	void Start()
	{
		cameraRot = cam.transform.localRotation;
		characterRot = transform.localRotation;
		lookAction.action.Enable();
	}

	void LateUpdate()
	{
		HandleLook();
		//UpdateCursorLock();	//カーソルの表示非表示
	}

	void HandleLook()
	{
		Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

		// 入力デバイスを判定
		float currentSensitivity = mouseSensitivity;

		if (lookAction.action.activeControl != null &&
			lookAction.action.activeControl.device is Gamepad)
		{
			currentSensitivity = gamepadSensitivity;
		}

		float xRot = lookInput.x * currentSensitivity;
		float yRot = lookInput.y * currentSensitivity;

		cameraRot *= Quaternion.Euler(-yRot, 0, 0);
		characterRot *= Quaternion.Euler(0, xRot, 0);

		cameraRot = ClampRotation(cameraRot);

		cam.transform.localRotation = cameraRot;
		transform.localRotation = characterRot;
	}
	//ゲーム画面をクリックするとカーソルを非表示にする
	public void UpdateCursorLock()
    {
		if (Input.GetKeyDown(KeyCode.Escape)) cursorLock = false;
		else if (Input.GetMouseButton(0)) cursorLock = true;

		Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;
	}

	//角度制限関数の作成
	public Quaternion ClampRotation(Quaternion q)
	{
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

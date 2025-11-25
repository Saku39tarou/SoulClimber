
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
/*
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPOV : MonoBehaviour
{
	[SerializeField] private Transform playerTarget;
	[SerializeField] private Transform ghostTarget;
	[SerializeField] private float mouseSensitivity = 0.1f;
	[SerializeField] private float gamepadSensitivity = 3.0f;
	[SerializeField] private InputActionReference lookAction;
	[SerializeField] private float minX = -90f;
	[SerializeField] private float maxX = 90f;

	private Quaternion cameraRot, characterRot;
	private Transform currentTarget;

	void Start()
	{
		currentTarget = playerTarget;
		cameraRot = transform.localRotation;
		characterRot = transform.localRotation;
		lookAction.action.Enable();
	}

	void LateUpdate()
	{
		if (currentTarget != null)
		{
			// 位置追従
			transform.position = currentTarget.position;
		}
		HandleLook();
	}

	void HandleLook()
	{
		Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

		float currentSensitivity = mouseSensitivity;

		var activeControl = lookAction.action.activeControl;
		if (activeControl != null && activeControl.device is Gamepad)
			currentSensitivity = gamepadSensitivity;

		float xRot = lookInput.x * currentSensitivity;
		float yRot = lookInput.y * currentSensitivity;

		cameraRot *= Quaternion.Euler(-yRot, 0, 0);
		characterRot *= Quaternion.Euler(0, xRot, 0);

		cameraRot = ClampRotation(cameraRot);

		transform.localRotation = characterRot;
		transform.GetChild(0).localRotation = cameraRot; 
	}

	public void SwitchToPlayer() => currentTarget = playerTarget;
	public void SwitchToGhost() => currentTarget = ghostTarget;

	Quaternion ClampRotation(Quaternion q)
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
*/
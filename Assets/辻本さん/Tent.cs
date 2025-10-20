/*
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

public class Tent : MonoBehaviour
{
	[SerializeField] GameObject climbPlayer;
	[SerializeField] GameObject ghostPlayer;
	[SerializeField] GameObject waitPos;
	bool onPlayer = false;
	bool onGhost = false;

	[SerializeField] PlayerController.State changeState;
	// Start is called before the first frame update
	void Start()
    {
        ghostPlayer.SetActive(false);
		
		
	}

    // Update is called once per frame
    void Update()
    {

		if (onPlayer && Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("ノーマルでEが押された");
			climbPlayer.transform.position = waitPos.transform.position;
			ghostPlayer.SetActive(true);
			climbPlayer.SetActive(false);
			GameObject.FindWithTag("Ghost").GetComponent<PlayerController>().SetState(changeState);
			changeState = PlayerController.State.Walk;
		}

		if (onGhost && Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("ゴーストでEが押された");
			ghostPlayer.transform.position = waitPos.transform.position;
			ghostPlayer.SetActive(false);
			climbPlayer.SetActive(true);
			GameObject.FindWithTag("Player").GetComponent<PlayerController>().SetState(changeState);
			changeState = PlayerController.State.Ghost;
		}

	}

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			onPlayer = true;
		}

		if(other.gameObject.CompareTag("Ghost"))
		{
			onGhost = true;
		}
	}
}
*/
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

public class Tent : MonoBehaviour
{
	[SerializeField] private GameObject climbPlayer;
	[SerializeField] private GameObject ghostPlayer;
	[SerializeField] private Transform waitPos;

	private bool onPlayer = false;
	private bool onGhost = false;

	void Start()
	{
		ghostPlayer.SetActive(false);
	}

	void Update()
	{
		// 通常プレイヤー → ゴーストへ
		if (onPlayer && Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("ノーマルでEが押された");

			// プレイヤーを待機地点に
			climbPlayer.transform.position = waitPos.position;
			climbPlayer.SetActive(false);

			// ゴースト出現
			ghostPlayer.transform.position = waitPos.position;
			ghostPlayer.SetActive(true);

			// ゴーストのStateをGhostに
			var ghostController = ghostPlayer.GetComponent<PlayerController>();
			if (ghostController != null)
				ghostController.SetState(PlayerController.State.Ghost);
		}

		// ゴースト → 通常プレイヤーへ
		if (onGhost && Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("ゴーストでEが押された");

			ghostPlayer.transform.position = waitPos.position;
			ghostPlayer.SetActive(false);

			climbPlayer.transform.position = waitPos.position;
			climbPlayer.SetActive(true);

			// プレイヤーのStateをWalkに
			var playerController = climbPlayer.GetComponent<PlayerController>();
			if (playerController != null)
				playerController.SetState(PlayerController.State.Normal);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) onPlayer = true;
		if (other.CompareTag("Ghost")) onGhost = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player")) onPlayer = false;
		if (other.CompareTag("Ghost")) onGhost = false;
	}
}

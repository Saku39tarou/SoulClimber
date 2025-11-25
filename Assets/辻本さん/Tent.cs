using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Tent : MonoBehaviour
{
	[SerializeField] private GameObject climbPlayer;
	[SerializeField] private GameObject ghostPlayer;
	[SerializeField] private Transform waitPos;
	//[SerializeField] private Transform target;

	private bool onPlayer = false;
	private bool onGhost = false;

	void Start()
	{
		ghostPlayer.SetActive(false);
	}

	void Update()
	{
		// 通常プレイヤーからゴーストへ
		if (onPlayer && Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("プレイヤーでEが押された");

			// プレイヤーを待機地点に
			climbPlayer.transform.position = waitPos.position;
			climbPlayer.SetActive(false);

			// ゴースト出現
			ghostPlayer.transform.position = waitPos.position;
			ghostPlayer.SetActive(true);

			// ゴーストのStateをGhostに
			PlayerController ghostController = ghostPlayer.GetComponent<PlayerController>();
			if (ghostController != null)
				ghostController.SetState(PlayerController.State.Ghost);
		}

		// ゴーストから通常プレイヤーへ
		if (onGhost && Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("ゴーストでEが押された");

			ghostPlayer.transform.position = waitPos.position;
			ghostPlayer.SetActive(false);

			climbPlayer.transform.position = waitPos.position;
			climbPlayer.SetActive(true);

			// プレイヤーのStateをWalkに
			PlayerController playerController = climbPlayer.GetComponent<PlayerController>();
			if (playerController != null)
				playerController.SetState(PlayerController.State.Normal);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			onPlayer = true;
			onGhost = false;
		}

		if (other.CompareTag("Ghost"))
		{
			onGhost = true;
			onPlayer = false;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player")) onPlayer = false;
		if (other.CompareTag("Ghost")) onGhost = false;
	}
}

using UnityEngine;

public class Tent : MonoBehaviour
{
	[SerializeField] private GameObject climbPlayer;
	[SerializeField] private GameObject ghostPlayer;
	[SerializeField] private Transform waitPos;

	[SerializeField] private SkySystem skySystem;

	private bool onPlayer = false;
	private bool onGhost = false;

	private SkySystem.Sky prevSkyState;

	void Start()
	{
		ghostPlayer.SetActive(false);

		if (skySystem != null)
			prevSkyState = skySystem.skyState;
	}

	void Update()
	{
		if (skySystem != null)
		{
			bool becameMorning = (prevSkyState == SkySystem.Sky.Night && skySystem.skyState == SkySystem.Sky.Day);

			if (becameMorning)
			{
				if (ghostPlayer.activeSelf)
				{
					ForceBackToPlayer();
				}
			}

			prevSkyState = skySystem.skyState;
		}

		// 通常プレイヤー → ゴースト
		if (onPlayer && Input.GetKeyDown(KeyCode.E))
		{
			climbPlayer.transform.position = waitPos.position;
			climbPlayer.SetActive(false);

			ghostPlayer.transform.position = waitPos.position;
			ghostPlayer.SetActive(true);

			var ghostController = ghostPlayer.GetComponent<PlayerController>();
			if (ghostController != null)
				ghostController.SetState(PlayerController.State.Ghost);
		}

		// ゴースト → 通常プレイヤー
		if (onGhost && Input.GetKeyDown(KeyCode.E))
		{
			ghostPlayer.transform.position = waitPos.position;
			ghostPlayer.SetActive(false);

			climbPlayer.transform.position = waitPos.position;
			climbPlayer.SetActive(true);

			var playerController = climbPlayer.GetComponent<PlayerController>();
			if (playerController != null)
				playerController.SetState(PlayerController.State.Normal);
		}
	}

	// 朝になったら強制的にPlayerに戻す
	private void ForceBackToPlayer()
	{
		Debug.Log("朝になったので、ゴーストを強制的にPlayerへ戻します");


		ghostPlayer.transform.position = waitPos.position;
		ghostPlayer.SetActive(false);

		climbPlayer.transform.position = waitPos.position;
		climbPlayer.SetActive(true);

		
		var playerController = climbPlayer.GetComponent<PlayerController>();
		if (playerController != null)
			playerController.SetState(PlayerController.State.Normal);

		onGhost = false;
		onPlayer = false;
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
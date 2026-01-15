using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverText : MonoBehaviour
{
	[SerializeField] static Canvas gameOverCanvas;

	private void Awake()
	{
		gameOverCanvas = GetComponent<Canvas>();
	}

	public static void GameOverShowPanel()
	{
		Time.timeScale = 0f;
		gameOverCanvas.enabled = true;
	}

	public void ReStartGame()
	{
		Time.timeScale = 1f;

		SceneManager.LoadScene(0);
	}

}

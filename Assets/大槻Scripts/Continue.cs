using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
	public void change_button()
	{
		Time.timeScale = 1f;
		this.gameObject.SetActive(false);
		SceneManager.LoadScene("GameScene");
	}
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector0 : MonoBehaviour
{
	public GameObject TimerText;//timerText更新
	float time = 300.0f;//制限時間
	
	void Start()
	{
		Time.timeScale = 1f;
		this.TimerText = GameObject.Find("TimerText");//指定したテキストを更新
	}

	// Update is called once per frame
	void Update()
	{
		this.time -= Time.deltaTime;
		this.TimerText.GetComponent<TMP_Text>().text =
			this.time.ToString("F1");
	}
}

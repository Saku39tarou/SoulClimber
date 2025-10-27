using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
	//幽体離脱中に表示するカウントダウン
	private float counttime = 0.0f;//時間計測
	public float timeLimit = 300.0f;//5分

	// Update is called once per frame
	void Update()
	{
		counttime += Time.deltaTime;//かかった時間を計測する

		//間に合わなければゲームオーバーになり　シーンに遷移される

		if (counttime > timeLimit)
		{
			SceneManager.LoadScene("TitleScene");//何秒後にシーン遷移
		}
	}
}

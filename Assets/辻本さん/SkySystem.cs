using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SkySystem : MonoBehaviour
{
	public string[] SkyNames;

	//角度を入れる変数
	[SerializeField]
	private float sunPos;

	[SerializeField]
	TextMeshProUGUI skySituation;

	Coroutine fadeIn;
	public int dayCount = 1;

	enum Sky
	{
		Day,
		Night,
	}
	[SerializeField] Sky sky;

	void Start()
	{
		SkyNames = new string[] {"朝","もうすぐ日が沈む....","夜","もうすぐ夜が明ける..."};
		skySituation.text = "";
	}

	void Update()
	{
		//X軸を回転させる
		transform.eulerAngles = new Vector3(sunPos, 0, 0);

		//1日のスピードを調節する
		sunPos += Time.deltaTime * 2;

		if (sunPos >= 150)
		{
			skySituation.text = SkyNames[1];
			
			StartCoroutine("FadeIn");
		}


		if (sunPos >= 180)
		{
			skySituation.text = SkyNames[2];
			
			StartCoroutine("FadeIn");
			sky = Sky.Night;
		}

		if (sunPos >= 330)
		{
			skySituation.text = SkyNames[3];
			
			StartCoroutine("FadeIn");
			
		}

		if (sunPos >= 360)
		{
			skySituation.text = SkyNames[0];
			
			StartCoroutine("FadeIn");
			sunPos = 0;
			sky = Sky.Day;
		}
	}

	
	IEnumerator FadeIn()
	{
		skySituation.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		while (true)
		{
			for (int i = 0; i < 255; i++)
			{
				skySituation.color = skySituation.color + new Color32(0, 0, 0, 1);
				yield return new WaitForSeconds(0.1f);
			}
		}
		
	}

	IEnumerator FadeOut()
	{
		while (true)
		{
			for (int i = 225; i > 1; i--)
			{
				skySituation.color = skySituation.color - new Color32(0, 0, 0, 1);
				yield return new WaitForSeconds(0.5f);
			}
		}
	}
}
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
	private float alpha;
	[SerializeField]
	TextMeshProUGUI skySituation;
	[SerializeField]
	GameObject fadeSwich;


	[SerializeField] bool fadeIn;
	[SerializeField] bool fadeOut;

	private float nextWaitTime;
	public int dayCount = 1;

	enum Sky
	{
		Day,
		Night,
	}
	[SerializeField] Sky sky;


	void Start()
	{
		fadeIn = true;
		fadeOut = false;
		skySituation.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		SkyNames = new string[] { "朝", "もうすぐ日が沈む....", "夜", "もうすぐ夜が明ける..." };
		skySituation.text = "";
	}

	void Update()
	{
		//X軸を回転させる
		transform.eulerAngles = new Vector3(sunPos, 0, 0);

		//1日のスピードを調節する
		sunPos += Time.deltaTime;

		if (sunPos >= 160 && sunPos <= 170)
		{
			skySituation.text = SkyNames[1];
			if (fadeIn) FadeIn();
			if (fadeOut) FadeOut();
		}
		if (sunPos >= 180 && sunPos <= 190)
		{

			skySituation.text = SkyNames[2];
			if (fadeIn) FadeIn();
			if (fadeOut) FadeOut();
			sky = Sky.Night;
		}
		if (sunPos >= 330 && sunPos <= 340)
		{

			skySituation.text = SkyNames[3];
			if (fadeIn) FadeIn();
			if (fadeOut) FadeOut();
		}
		if (sunPos >= 360)
		{

			skySituation.text = SkyNames[0];
			if (fadeIn) FadeIn();
			if (fadeOut) FadeOut();
			sunPos = 0;
			sky = Sky.Day;
		}

	}


	private void FadeIn()
	{
		//for (int i = 0; i < 255; i++)
		//{
		//	skySituation.color = skySituation.color + new Color32(0, 0, 0, 1);
		//	new WaitForSeconds(0.5f);
		//}
		alpha += 0.001f;
		skySituation.color = new Color(0, 0, 0, alpha);
		if (alpha >= 1)
		{
			nextWaitTime += Time.deltaTime;
			if (nextWaitTime >= 3.0f)
			{
				nextWaitTime = 0;
				fadeIn = false;
				fadeOut = true;
			}

		}
	}

	void FadeOut()
	{
		//for (int i = 255; i > 1; i--)
		//{
		//	skySituation.color = skySituation.color - new Color32(0, 0, 0, 1);
		//	new WaitForSeconds(0.1f);
		//}
		alpha -= 0.001f;
		skySituation.color = new Color(0, 0, 0, alpha);
		if (alpha <= 0)
		{
			fadeOut = false;

		}
	}
}
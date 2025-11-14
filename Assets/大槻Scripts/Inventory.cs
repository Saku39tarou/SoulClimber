using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	//
	/*
	public GameObject HealItem1;
	public GameObject HealItem2;
	//public GameObject HealItem3;

	Image HealItemPoint1;
	Image HealItemPoint2;

	void Start()
	{
		
		HealItemPoint1 = GameObject.Find("HealItemPoint1").GetComponent<Image>();
		HealItemPoint2 = GameObject.Find("HealItemPoint2").GetComponent<Image>();


		HealItemPoint1.enabled = false;
		HealItemPoint2.enabled = false;
		
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag=="HealItemPoint1")
		{
			HealItemPoint1.enabled = true;
		}
		if (col.gameObject.tag == "HealItemPoint2")
		{
			HealItemPoint2.enabled = true;
		}
	}
	*/
	//

	[SerializeField] GameObject Button;

	//private int potionHealAmount = 0;
	private float potionHealAmount = 0;
	private bool hasPotion = false;

	public void AddPotion(float healAmount)
	{
		potionHealAmount = healAmount;
		hasPotion = true;
		Debug.Log("アイテムを取得しました！");
	}

	public void UsePotion()
	{
		if (hasPotion)
		{
			PlayerHealth hp = GetComponent<PlayerHealth>();
			hp.Heal(potionHealAmount);
			hasPotion = false;
			Debug.Log("アイテムを使用しました！");
			Button.SetActive(false);//アイコンをクリックすると使用しアイコンを削除
		}
		else
		{
			Debug.Log("アイテムがありません！");
		}
	}
}



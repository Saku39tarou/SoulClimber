using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	

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



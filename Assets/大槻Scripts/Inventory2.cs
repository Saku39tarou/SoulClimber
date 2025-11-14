using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory2 : MonoBehaviour
{
	[SerializeField] GameObject Button2;

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
			Button2.SetActive(false);//アイコンをクリックすると使用しアイコンを削除
		}
		else
		{
			Debug.Log("アイテムがありません！");
		}
	}
}

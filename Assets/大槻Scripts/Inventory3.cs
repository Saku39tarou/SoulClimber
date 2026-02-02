using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory3 : MonoBehaviour
{
	[SerializeField] GameObject Button3;

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
			PlayerHealth1 hp = GetComponent<PlayerHealth1>();
			hp.Heal(potionHealAmount);
			hasPotion = false;
			Debug.Log("アイテムを使用しました！");
			Button3.SetActive(false);//アイコンをクリックすると使用しアイコンを削除
		}
		else
		{
			Debug.Log("アイテムがありません！");
		}
	}
}

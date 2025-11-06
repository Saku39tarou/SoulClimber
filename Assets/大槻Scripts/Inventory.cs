using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	//private int potionHealAmount = 0;
	private float potionHealAmount = 0;
	private bool hasPotion = false;

	public void AddPotion(float healAmount)
	{
		potionHealAmount = healAmount;
		hasPotion = true;
		Debug.Log("ポーションを取得しました！");
	}

	public void UsePotion()
	{
		if (hasPotion)
		{
			PlayerHealth hp = GetComponent<PlayerHealth>();
			hp.Heal(potionHealAmount);
			hasPotion = false;
			Debug.Log("ポーションを使用しました！");
		}
		else
		{
			Debug.Log("ポーションがありません！");
		}
	}
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFind : MonoBehaviour
{
	//public string itemName = "Potion";
	public string itemName = "Recovery";

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			// プレイヤーが近づいた時
			Debug.Log($"{itemName}を拾う");
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))//タグがPlayerでEキーを押すと拾うことができる
		{
			InventoryManager.Instance.AddItem(itemName);
			Destroy(gameObject); // 拾って消す
		}
	}
}



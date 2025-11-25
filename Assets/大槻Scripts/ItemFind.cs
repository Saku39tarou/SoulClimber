using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFind : MonoBehaviour
{
	/*
	[SerializeField] bool onhidhlight = true;
	[SerializeField] GameObject recovery;
	*/


	//public string itemName = "Potion";
	public string itemName = "Recovery";

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag=="Player")
		{
			// プレイヤーが近づいた時
			Debug.Log($"{itemName}を拾う");
			
		}
	}
	
	private void OnTriggerStay(Collider collision)
	{
		if (collision.gameObject.tag=="Player" && Input.GetKeyDown(KeyCode.Q))//タグがPlayerでQキーを押すと拾うことができる
		{
			InventoryManager.Instance.AddItem(itemName);
			Destroy(gameObject); // 拾って消す
		}
	}
}



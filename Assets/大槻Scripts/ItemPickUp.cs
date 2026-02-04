using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemPickup : MonoBehaviour
{
	[SerializeField] GameObject Button;

	//public int healAmount = 30;
	public float healAmount = 0.1f;


	private void Start()
	{
		Button.SetActive(false);
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")||other.CompareTag("Ghost"))
		{
			Inventory inventory = other.GetComponent<Inventory>();
			if (inventory != null)
			{
				inventory.AddPotion(healAmount);
				//Destroy(gameObject); // 拾ったらアイテム消える
				
			}
			
			Button.SetActive(true);//アイテム取得するまで非表示
			Debug.Log("アイコン表示");
		}

		if (other.gameObject.tag == "Player")
		{
			// プレイヤーが近づいた時
			Debug.Log($"{itemName}を拾う");

		}

	}
	public string itemName = "Recovery";


	private void OnTriggerStay(Collider collision)
	{
		if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.Q))//タグがPlayerでQキーを押すと拾うことができる
		{
			InventoryManager.Instance.AddItem(itemName);
			Destroy(gameObject); // 拾って消す
			Button.SetActive(true);//アイテム取得するまで非表示
			Debug.Log("アイコン表示");
		}
	}

}



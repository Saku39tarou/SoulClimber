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
		if (other.CompareTag("Player"))
		{
			Inventory inventory = other.GetComponent<Inventory>();
			if (inventory != null)
			{
				inventory.AddPotion(healAmount);
				Destroy(gameObject); // 拾ったらアイテム消える
			}

			Button.SetActive(true);//アイテム取得するまで非表示
			Debug.Log("アイコン表示");
		}
	}

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItempickUp3 : MonoBehaviour
{
	[SerializeField] GameObject Button3;
	//public int healAmount = 30;
	public float healAmount = 0.4f;


	private void Start()
	{
		Button3.SetActive(false);
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") || other.CompareTag("ghost"))
		{
			Inventory3 inventory = other.GetComponent<Inventory3>();
			if (inventory != null)
			{
				inventory.AddPotion(healAmount);
				//Destroy(gameObject); // 拾ったらアイテム消える
			}

			Button3.SetActive(true);//アイテム取得するまで非表示
			Debug.Log("アイコン表示");
		}

	}
		
}

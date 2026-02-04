using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp2 : MonoBehaviour
{
	[SerializeField] GameObject Button2;

	//public int healAmount = 30;
	public float healAmount = 0.1f;


	private void Start()
	{
		Button2.SetActive(false);
	}

	[SerializeField] AudioClip se;
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")||other.CompareTag("ghost"))
		{
			Inventory2 inventory = other.GetComponent<Inventory2>();
			if (inventory != null)
			{
				inventory.AddPotion(healAmount);
				//Destroy(gameObject); // 拾ったらアイテム消える
			}
			AudioSource.PlayClipAtPoint(se, transform.position);
			Button2.SetActive(true);//アイテム取得するまで非表示
			Debug.Log("アイコン表示");
		}

	}
}

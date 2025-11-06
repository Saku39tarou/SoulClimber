using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemPickup : MonoBehaviour
{
	//public int healAmount = 30;
	public float healAmount = 0.1f;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Inventory inventory = other.GetComponent<Inventory>();
			if (inventory != null)
			{
				inventory.AddPotion(healAmount);
				Destroy(gameObject); // èEÇ¡ÇΩÇÁÉAÉCÉeÉÄè¡Ç¶ÇÈ
			}
		}
	}
}



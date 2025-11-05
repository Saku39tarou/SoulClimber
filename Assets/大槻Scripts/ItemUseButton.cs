using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsePotionButton : MonoBehaviour
{
	public Button useButton;

	private void Start()
	{
		useButton.onClick.AddListener(UsePotion);
	}

	void UsePotion()
	{
		//InventoryManager.Instance.UseItem("Potion");
		InventoryManager.Instance.UseItem("Recovery");
	}
}



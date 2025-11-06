using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealItem : MonoBehaviour
{
	public string itemName;
	public Sprite icon;
	public ItemType type;
	public float healAmount; // ‰ñ•œ—Ê

	public enum ItemType
	{
		Heal,
		Other
	}
}



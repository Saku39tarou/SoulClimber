using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

//[Serializable]
//[CreateAssetMenu(fileName ="Item",menuName ="CreateItem")]

public class Items : ScriptableObject//MonoBehaviour 
{
	
	public enum KindOfItem
	{
		Weapon,
		Recovery,
		UseItem
	}

	//アイテムの種類
	[SerializeField]
	private KindOfItem kindOfItem;

	//　アイテムのアイコン
	[SerializeField]
	private Sprite icon;

	//　アイテムの名前
	[SerializeField]
	private string ItemName;

	//　アイテムの情報
	[SerializeField]
	private string information;

	public KindOfItem GetKindOfItem()
	{
		return kindOfItem;
	}
	
	public Sprite GetIcon()
	{
		return icon;
	}
	
	public string GetItemName()
	{
		return ItemName;
	}
	
	public string GetInformation()
	{
		return information;
	}
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "CreateItemDataBase")]
public class ItemDataBase : ScriptableObject
{
	
	[SerializeField]
	private List<Items> itemLists = new List<Items>();

	//　アイテムリストを返す
	public List<Items> GetItemLists()
	{
		return itemLists;
	}
	
}

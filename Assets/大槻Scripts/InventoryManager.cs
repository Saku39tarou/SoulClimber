using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
	public static InventoryManager Instance;

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	private List<string> items = new List<string>();
	
	public void AddItem(string itemName)
	{
		items.Add(itemName);
		Debug.Log(itemName + "を取得した！");
	}

	public bool HasItem(string itemName)
	{
		return items.Contains(itemName);
		

	}

	public void UseItem(string itemName)
	{
		if (items.Contains(itemName))
		{
			items.Remove(itemName);
			Debug.Log(itemName + "を使用");
		}
		else
		{
			Debug.Log(itemName + "を持っていません。");
		}
	}
}



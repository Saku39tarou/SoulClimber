using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
	public GameObject itemPrefab; 
	public Transform contentPanel; 

	public void AddItem(string itemName)
	{
		// プレハブを生成
		GameObject newItem = Instantiate(itemPrefab, contentPanel);

		// アイテムの中身（名前やアイコン）を更新する処理（例）
		newItem.GetComponentInChildren<Text>().text = itemName;

		// （オプション）クリック時のイベントを追加
		newItem.GetComponent<Button>().onClick.AddListener(() => Debug.Log(itemName + " clicked"));
	}
}

/*
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemPanel : MonoBehaviour
{
	[SerializeField] string itemName;
	[SerializeField] Sprite icon;
	[SerializeField] int maxStack = 5; //最大個数
	

	public class Inventory : MonoBehaviour
	{
		public List<Item> items = new List<Item>();

		public void AddItem(Item item)
		{
			// アイテムを追加する処理
			items.Add(item);
		}
	}

}*/



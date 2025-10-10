using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemData : MonoBehaviour
{
	[SerializeField] private ItemList itemList;					// ScriptableObjectをアタッチ
	[SerializeField] private int itemId;						// 表示したいアイテムのID
	[SerializeField] private TMP_Text itemNameText;				// アイテム名表示用
	[SerializeField] private TMP_Text itemEffectValueText;      // 効果値表示用

	private void Start()
	{
		DisplayItemDetails(itemId);
	}

	private void DisplayItemDetails(int id)
	{
		// IDに該当するアイテムを取得
		Item item = itemList.GetItemById(id);

		if (item != null)
		{
			itemNameText.text = item.itemName;
			itemEffectValueText.text = $"Effect: {item.effectValue}";
		}
	}
}

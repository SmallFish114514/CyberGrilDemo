using System;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InventoryItemUI : MonoBehaviour, IController
{
	private Image itemImage;

	public DropItemInfo dropItem;

	public IDropItemSystem dropItemSystem;

	private Text itemText;

	public int Amount { get; private set; }

	private Text AmountText
	{
		get
		{
			if (itemText == null)
			{
				itemText = GetComponentInChildren<Text>();
			}
			return itemText;
		}
	}

	private void Awake()
	{
		dropItemSystem = this.GetSystem<IDropItemSystem>();
	}

	public void SetItem(int id, int amount = 1)
	{
		Amount = amount;
		AmountText.text = Amount.ToString();
		itemImage = transform.GetComponent<Image>();
		if (itemImage != null)
		{
			dropItem = this.GetSystem<IDropItemSystem>().GetDropItemById(id);
			Sprite sprite = Resources.Load<Sprite>("Inventory/" + dropItem.AssetPath);
			if (sprite != null)
			{
				itemImage.sprite = sprite;
			}
			else
			{
				Debug.LogWarning("Not Exist Image!!!");
			}
		}
	}

	public void AddItem(int amount = 1)
	{
		Amount += amount;
		AmountText.text = Amount.ToString();
	}

	public void RemoveItem(int amount = 1)
	{
		Amount -= amount;
		AmountText.text = Amount.ToString();
	}

	public void RemoveItem(InventoryItemUI item, int amount = 1)
	{
		Amount -= amount;
		AmountText.text = Amount.ToString();
		if (item.Amount <= 0)
		{
			Destroy((item).gameObject);
		}
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}
}

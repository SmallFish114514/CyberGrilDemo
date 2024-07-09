using System;
using QFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IController
{
	private GameObject ItemGo;

	private ToolTipInfo toolTip;

	private bool isTipInfo = false;

	public InventoryItemUI currentItem;

	public int itemId;

	private ToolTip_Discard DiscardToolTip;

	public GameObject itemClo;

	private void Awake()
	{
		ItemGo = Resources.Load<GameObject>("Inventory/DropItemUI");
		toolTip = FindObjectOfType<ToolTipInfo>();
		DiscardToolTip = FindObjectOfType<ToolTip_Discard>(true);
		DiscardToolTip.gameObject.SetActive(false);
	}

	private void Start()
	{
		if (DiscardToolTip == null)
		{
			return;
		}
		(DiscardToolTip.ConfirmButton.onClick).AddListener(() =>
		{
			if (currentItem != null)
			{
				currentItem.RemoveItem(currentItem, DiscardToolTip.GetCustomDiscardAmount());
			}
			Inventory.Instance.isSelectItem = false;
			DiscardToolTip.gameObject.SetActive(false);
		});
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Inventory.Instance.isSelectItem && !EventSystem.current.IsPointerOverGameObject(-1))
		{
			GameManager.Instance.PlayClikAudio();
			DiscardToolTip.gameObject.SetActive(true);
			Inventory.Instance.HidePickUpItem();
		}
		else if (isTipInfo)
		{
			Transform root = transform.root;
			RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)((root is RectTransform) ? root : null), Input.mousePosition, null, out Vector2 Pos);
			toolTip.SetPos(Pos + new Vector2(30f, -20f));
		}
		else if (Inventory.Instance.isSelectItem)
		{
			Transform root2 = transform.root;
			RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)((root2 is RectTransform) ? root2 : null), Input.mousePosition, null, out Vector2 Pos);
			Inventory.Instance.SetSelectItemPos(Pos);
		}
	}

	public void StoreItem(int id)
	{
		itemId = id;
		if (transform.childCount == 0)
		{
			itemClo = Instantiate(ItemGo);
			itemClo.transform.SetParent(transform);
			itemClo.transform.position = transform.position;
			itemClo.transform.localScale = Vector3.one;
			itemClo.GetComponent<InventoryItemUI>().SetItem(id);
		}
		else
		{
			(transform.GetChild(0)).GetComponent<InventoryItemUI>().AddItem();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (transform.childCount > 0)
		{
			InventoryItemUI dropItem = (transform.GetChild(0)).GetComponent<InventoryItemUI>();
			isTipInfo = true;
			toolTip.ShowToolTip(dropItem.dropItemSystem.GetDropItemInfoById(itemId));
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (transform.childCount > 0)
		{
			isTipInfo = false;
			toolTip.HideToolTip();
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (transform.childCount > 0)
		{
			currentItem = (transform.GetChild(0)).GetComponent<InventoryItemUI>();
		}
		if (Input.GetMouseButtonDown(0))
		{
			GameManager.Instance.PlayClikAudio();
			if (transform.childCount > 0)
			{
				if (!Inventory.Instance.isSelectItem)
				{
					Inventory.Instance.PickUpItem(itemId);
					currentItem.RemoveItem(currentItem);
					Inventory.Instance.ShowPickUpItem();
					toolTip.HideToolTip();
				}
				else if (itemId == Inventory.Instance.SelectItem.dropItem.Id)
				{
					currentItem.AddItem();
					Inventory.Instance.RemoveSelectItem();
				}
				else
				{
					DropItemInfo item = currentItem.dropItem;
					int currentAmount = currentItem.Amount;
					currentItem.SetItem(Inventory.Instance.SelectItem.dropItem.Id, Inventory.Instance.SelectItem.Amount);
					Inventory.Instance.SelectItem.SetItem(item.Id, currentAmount);
				}
			}
			else if (Inventory.Instance.isSelectItem)
			{
				StoreItem(Inventory.Instance.SelectItem.dropItem.Id);
				Inventory.Instance.RemoveSelectItem();
			}
		}
		else if (Input.GetMouseButtonDown(1) && transform.childCount > 0)
		{
			GameManager.Instance.PlayClikAudio();
			if (currentItem.dropItem.Id == 1)
			{
				this.SendCommand<PickUpHpCommand>();
			}
			else if (currentItem.dropItem.Id == 2)
			{
				this.SendCommand<PickUpAmmoCommand>();
			}
			currentItem.RemoveItem(currentItem);
			toolTip.HideToolTip();
		}
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}
}

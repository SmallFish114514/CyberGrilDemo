using UnityEngine;

public class Inventory : MonoBehaviour
{
	public Slot[] slots;

	public bool isSelectItem = false;

	public InventoryItemUI SelectItem;

	public static Inventory Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
		slots = transform.GetComponentsInChildren<Slot>();
		SelectItem.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		FindNotEmptySlot();
	}

	private void Start()
	{
	}

	public void StoreItem(int id)
	{
		Slot slot = FindSameIDSlot(id);
		if (slot != null)
		{
			slot.StoreItem(id);
			return;
		}
		Slot emptySlot = FindEmptySlot();
		if (emptySlot != null)
		{
			emptySlot.StoreItem(id);
		}
		else
		{
			Debug.Log("NOT HAVE EMPTYSLOT!!!");
		}
	}

	public Slot FindEmptySlot()
	{
		Slot[] array = slots;
		foreach (Slot slot in array)
		{
			if ((slot).transform.childCount == 0)
			{
				return slot;
			}
		}
		return null;
	}

	public void FindNotEmptySlot()
	{
		Slot[] array = slots;
		foreach (Slot slot in array)
		{
			if ((slot).transform.childCount != 0)
			{
				InventoryManager.instance.Slots.Add(slot);
			}
		}
	}

	private Slot FindSameIDSlot(int id)
	{
		Slot[] array = slots;
		foreach (Slot slot in array)
		{
			if ((slot).transform.childCount > 0 && slot.itemId == id)
			{
				return slot;
			}
		}
		return null;
	}

	public void PickUpItem(int id, int amount = 1)
	{
		SelectItem.SetItem(id, amount);
		isSelectItem = true;
	}

	public void ShowPickUpItem()
	{
		SelectItem.gameObject.SetActive(true);
	}

	public void HidePickUpItem()
	{
		SelectItem.gameObject.SetActive(false);
	}

	public void SetSelectItemPos(Vector3 Pos)
	{
		SelectItem.transform.localPosition = Pos;
	}

	public void RemoveSelectItem(int amount = 1)
	{
		SelectItem.RemoveItem(amount);
		if (SelectItem.Amount <= 0)
		{
			isSelectItem = false;
			HidePickUpItem();
		}
	}
}

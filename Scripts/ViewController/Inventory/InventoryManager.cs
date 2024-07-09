using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public static InventoryManager instance;

	public List<Slot> Slots = new List<Slot>();

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		instance = this;
	}

	public void ResetInventory()
	{
		foreach (Slot item in Slots)
		{
			Destroy((item).transform.GetChild(0));
		}
	}
}

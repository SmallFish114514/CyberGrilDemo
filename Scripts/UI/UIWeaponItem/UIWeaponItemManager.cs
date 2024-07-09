using UnityEngine;

public class UIWeaponItemManager : MonoBehaviour
{
	private WeaponSlot[] weaponSlots;

	public static UIWeaponItemManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
		weaponSlots = transform.GetComponentsInChildren<WeaponSlot>();
	}

	public void StoreWeaponItem(string weaponName)
	{
		weaponSlots = transform.GetComponentsInChildren<WeaponSlot>();
		if (weaponSlots == null)
		{
			return;
		}
		WeaponSlot[] array = weaponSlots;
		foreach (WeaponSlot item in array)
		{
			if ((item).transform.childCount > 0 && item.mName == weaponName)
			{
				return;
			}
		}
		WeaponSlot weaponSlot = FindEmptySlot();
		if (weaponSlot == null)
		{
			Debug.Log("Not Have EmptySlot");
		}
		else
		{
			weaponSlot.StoreItem(weaponName);
		}
	}

	private WeaponSlot FindEmptySlot()
	{
		WeaponSlot[] array = weaponSlots;
		foreach (WeaponSlot item in array)
		{
			if ((item).transform.childCount == 0)
			{
				return item;
			}
		}
		return null;
	}

	public void ClearSlot()
	{
		WeaponSlot[] array = weaponSlots;
		foreach (WeaponSlot item in array)
		{
			if (item.mName == "ShotGun")
			{
				Destroy(item);
				break;
			}
		}
	}
}

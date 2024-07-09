


// SaveDataModel
using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[Serializable]
public class SaveDataModel : AbstractModel, ISaveDataModel, IModel, IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
{
	public Vector3 playerPos;

	public int playerHp;

	public List<int> turretsHp = new List<int>();

	public List<int> dronesHp = new List<int>();

	public List<Vector3> turretsPos = new List<Vector3>();

	public List<Vector3> dronesPos = new List<Vector3>();

	public List<int> weaponsID = new List<int>();

	public List<int> weaponsBullet = new List<int>();

	public List<string> weaponsName = new List<string>();

	public List<int> inventoryItemsID = new List<int>();

	public List<int> itemsAmount = new List<int>();

	public Vector3 PlayerPos
	{
		get
		{
			return playerPos;
		}
		set
		{
			playerPos = value;
		}
	}

	public int PlayerHp
	{
		get
		{
			return playerHp;
		}
		set
		{
			playerHp = value;
		}
	}

	public List<int> TurretsHp
	{
		get
		{
			return turretsHp;
		}
		set
		{
			turretsHp = value;
		}
	}

	public List<int> DronesHp
	{
		get
		{
			return dronesHp;
		}
		set
		{
			dronesHp = value;
		}
	}

	public List<Vector3> TurretsPos
	{
		get
		{
			return turretsPos;
		}
		set
		{
			turretsPos = value;
		}
	}

	public List<Vector3> DronesPos
	{
		get
		{
			return dronesPos;
		}
		set
		{
			dronesPos = value;
		}
	}

	public List<int> WeaponsID
	{
		get
		{
			return weaponsID;
		}
		set
		{
			weaponsID = value;
		}
	}

	public List<int> WeaponsBullet
	{
		get
		{
			return weaponsBullet;
		}
		set
		{
			weaponsBullet = value;
		}
	}

	public List<string> WeaponsName
	{
		get
		{
			return weaponsName;
		}
		set
		{
			weaponsName = value;
		}
	}

	public List<int> InventoryItemsID
	{
		get
		{
			return inventoryItemsID;
		}
		set
		{
			inventoryItemsID = value;
		}
	}

	public List<int> ItemsAmount
	{
		get
		{
			return itemsAmount;
		}
		set
		{
			itemsAmount = value;
		}
	}

	protected override void OnInit()
	{
	}
}

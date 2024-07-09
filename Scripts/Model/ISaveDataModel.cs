


// ISaveDataModel
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public interface ISaveDataModel : IModel, IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
{
	int PlayerHp { get; set; }

	Vector3 PlayerPos { get; set; }

	List<int> TurretsHp { get; set; }

	List<Vector3> TurretsPos { get; set; }

	List<int> DronesHp { get; set; }

	List<Vector3> DronesPos { get; set; }

	List<int> WeaponsID { get; set; }

	List<int> WeaponsBullet { get; set; }

	List<string> WeaponsName { get; set; }

	List<int> InventoryItemsID { get; set; }

	List<int> ItemsAmount { get; set; }
}

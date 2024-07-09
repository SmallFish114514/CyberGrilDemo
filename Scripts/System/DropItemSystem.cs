using System;
using System.Collections.Generic;
using QFramework;

[Serializable]
public class DropItemSystem : AbstractSystem, IDropItemSystem, ISystem
{
	public Dictionary<int, DropItemInfo> DropItems { get; set; }

	public DropItemInfo GetDropItemById(int id)
	{
		return DropItems.ContainsKey(id) ? DropItems[id] : null;
	}

	public string GetDropItemInfoById(int id)
	{
		string color = "red";
		return string.Format("<color={0}>{1}</color>,<size=20><color={0}>{2}</color></size>", color, DropItems[id].Name, DropItems[id].Description);
	}

	protected override void OnInit()
	{
		DropItems = new Dictionary<int, DropItemInfo>
		{
			{
				1,
				new DropItemInfo("医疗箱", 1, 0, 50, "HP", "一次性消耗品,可增加50HP")
			},
			{
				2,
				new DropItemInfo("弹药箱", 2, 20, 0, "Ammo", "一次性消耗品,可增加20发弹药，手持枪械增加20发弹药")
			}
		};
	}
}

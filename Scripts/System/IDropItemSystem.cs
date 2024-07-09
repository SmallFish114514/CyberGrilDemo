using System.Collections.Generic;
using QFramework;

public interface IDropItemSystem : ISystem, IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetUtility, ICanRegisterEvent, ICanSendEvent
{
	Dictionary<int, DropItemInfo> DropItems { get; set; }

	DropItemInfo GetDropItemById(int id);

	string GetDropItemInfoById(int id);
}

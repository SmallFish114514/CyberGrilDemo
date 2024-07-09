using System.Collections.Generic;
using QFramework;

public interface IGunSystem : ISystem
{
	IGunConfigModel CurrentGun { get; set; }

	List<IGunConfigModel> GunList { get; set; }

	void PickGun(int id);

	void ShiftGun(int id);

	void ResetGun();
}

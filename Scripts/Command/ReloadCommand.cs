


// ReloadCommand
using QFramework;

public class ReloadCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		IGunConfigModel currentGun = this.GetSystem<IGunSystem>().CurrentGun;
		int needReloadBullet = currentGun.MaxBulletCountInGun - (int)currentGun.BulletInGun;
		if (needReloadBullet <= 0)
		{
			return;
		}
		currentGun.gunState.Value = GunState.Reload;
		if (currentGun.Name == "Pistol")
		{
			GameManager.Instance.PlayPistolReloadAudio();
		}
		else if (currentGun.Name == "Shotgun")
		{
			GameManager.Instance.PlayShotGunAudio();
		}
		this.GetSystem<ITimeSystem>().AddDelayTask(currentGun.CD, delegate
		{
			if ((int)currentGun.BulletCount >= needReloadBullet)
			{
				currentGun.BulletCount.Value -= needReloadBullet;
				currentGun.BulletInGun.Value += needReloadBullet;
			}
			else
			{
				currentGun.BulletInGun.Value += currentGun.BulletCount.Value;
				currentGun.BulletCount.Value = 0;
			}
			currentGun.gunState.Value = GunState.Idle;
		});
	}
}

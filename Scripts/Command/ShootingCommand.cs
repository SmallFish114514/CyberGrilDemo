using QFramework;

public class ShootingCommand : AbstractCommand
{
	public static readonly ShootingCommand Single = new ShootingCommand();

	private readonly int mShootBulletCount;

	private readonly float mShootInterval;

	public ShootingCommand(int ShootBulletCount = 1, float shootInterval = 0.8f)
	{
		mShootBulletCount = ShootBulletCount;
		mShootInterval = shootInterval;
	}

	protected override void OnExecute()
	{
		IGunConfigModel gunSystem = this.GetSystem<IGunSystem>().CurrentGun;
		gunSystem.BulletInGun.Value -= mShootBulletCount;
		gunSystem.gunState.Value = GunState.Shooting;
		this.GetSystem<ITimeSystem>().AddDelayTask(mShootInterval, delegate
		{
			gunSystem.gunState.Value = GunState.Idle;
			if (gunSystem.BulletInGun.Value == 0 && gunSystem.BulletCount.Value > 0)
			{
				this.SendCommand<ReloadCommand>();
			}
		});
	}
}

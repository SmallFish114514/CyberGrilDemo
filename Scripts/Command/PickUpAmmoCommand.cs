
using QFramework;

public class PickUpAmmoCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		GameManager.Instance.PlayUseItemAudio();
		IGunSystem gunSystem = this.GetSystem<IGunSystem>();
		IDropItemSystem dropItemSystem = this.GetSystem<IDropItemSystem>();
		gunSystem.CurrentGun.BulletCount.Value += dropItemSystem.GetDropItemById(2).AddAmmoValue;
	}
}

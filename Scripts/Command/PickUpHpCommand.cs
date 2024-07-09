


// PickUpHpCommand
using QFramework;

internal class PickUpHpCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		GameManager.Instance.PlayUseItemAudio();
		IPlayerModel playerModel = this.GetModel<IPlayerModel>();
		IDropItemSystem dropItemSystem = this.GetSystem<IDropItemSystem>();
		playerModel.HP.Value += dropItemSystem.GetDropItemById(1).AddHpValue;
	}
}

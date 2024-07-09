


// KillEnemyCommand
using QFramework;

public class KillEnemyCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.GetSystem<IStateSystem>().KillCount.Value++;
		this.GetSystem<IStateSystem>().KillCount.RegisterWithInitValue(delegate
		{
			GameUIManager.Instance.UpdateKillEnemyCount();
		});
	}
}

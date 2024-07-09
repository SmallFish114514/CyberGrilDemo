using QFramework;

public class AttackEnemyCommand : AbstractCommand
{
	private readonly int damageValue;

	private readonly int id;

	public AttackEnemyCommand(int EnemyID, int DamageVlaue = 1)
	{
		damageValue = DamageVlaue;
		id = EnemyID;
	}

	protected override void OnExecute()
	{
		DataManager.Instance.GetEnemyInfoByID(id).HP.Value -= damageValue;
	}
}

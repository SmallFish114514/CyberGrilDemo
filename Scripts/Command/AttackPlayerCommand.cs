


// AttackPlayerCommand
using QFramework;

public class AttackPlayerCommand : AbstractCommand
{
	private readonly int damageValue;

	public AttackPlayerCommand(int DamageValue = 1)
	{
		damageValue = DamageValue;
	}

	protected override void OnExecute()
	{
		this.GetModel<IPlayerModel>().HP.Value -= damageValue;
	}
}

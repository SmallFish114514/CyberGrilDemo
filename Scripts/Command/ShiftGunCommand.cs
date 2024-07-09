


// ShiftGunCommand
using QFramework;

public class ShiftGunCommand : AbstractCommand
{
	private readonly int id;

	public ShiftGunCommand(int mId)
	{
		id = mId;
	}

	protected override void OnExecute()
	{
		this.GetSystem<IGunSystem>().ShiftGun(id);
	}
}

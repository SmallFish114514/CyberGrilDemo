
using QFramework;

public class PickGunCommand : AbstractCommand
{
	private readonly int id;

	public PickGunCommand(int mId)
	{
		id = mId;
	}

	protected override void OnExecute()
	{
		this.GetSystem<IGunSystem>().PickGun(id);
	}
}

using QFramework;

public class StateSystem : AbstractSystem, IStateSystem, ISystem
{
	public BindableProperty<int> KillCount { get; } = new BindableProperty<int>(0)
	{
		Value = 0
	};


	protected override void OnInit()
	{
	}
}

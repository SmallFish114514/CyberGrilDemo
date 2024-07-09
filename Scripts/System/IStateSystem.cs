using QFramework;

public interface IStateSystem : ISystem
{
	BindableProperty<int> KillCount { get; }
}

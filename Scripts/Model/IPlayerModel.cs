


// IPlayerModel
using QFramework;

public interface IPlayerModel : IModel, IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
{
	string Name { get; }

	int MaxHp { get; }

	BindableProperty<int> HP { get; }

	int JumpForce { get; }

	float MoveSpeed { get; }

	BindableProperty<float> ActualSpeed { get; }

	float SprintSpeedMultiplier { get; }
}

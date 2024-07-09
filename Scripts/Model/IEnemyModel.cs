


// IEnemyModel
using QFramework;

public interface IEnemyModel : IModel, IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
{
	int ID { get; }

	string Name { get; }

	BindableProperty<int> HP { get; }

	int InitHP { get; set; }

	float MoveSpeed { get; }

	string Description { get; }

	int Damage { get; }

	float ItemDropChance { get; }

	ItemDropType DropType { get; }

	void ReSetHP();
}




// EnemyModel
using QFramework;

public class EnemyModel : AbstractModel, IEnemyModel, IModel, IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
{
	private int initHP;

	public BindableProperty<int> HP { get; }

	public int InitHP
	{
		get
		{
			return initHP;
		}
		set
		{
			initHP = value;
		}
	}

	public int ID { get; }

	public string Name { get; }

	public float MoveSpeed { get; }

	public string Description { get; }

	public int Damage { get; }

	public float ItemDropChance { get; }

	public ItemDropType DropType { get; }

	public EnemyModel()
	{
	}

	public EnemyModel(int id, string name, int hp, float moveSpeed, string description, int damage, float itemDropChance, ItemDropType dropType)
	{
		ID = id;
		Name = name;
		HP = new BindableProperty<int>(0)
		{
			Value = hp
		};
		MoveSpeed = moveSpeed;
		Description = description;
		Damage = damage;
		ItemDropChance = itemDropChance;
		DropType = dropType;
		initHP = HP.Value;
	}

	protected override void OnInit()
	{
	}

	public void ReSetHP()
	{
		HP.Value = InitHP;
	}
}

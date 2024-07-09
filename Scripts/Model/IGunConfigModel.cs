


// IGunConfigModel
using QFramework;

public interface IGunConfigModel : IModel, IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
{
	int ID { get; set; }

	string Name { get; set; }

	BindableProperty<int> BulletCount { get; set; }

	float CD { get; set; }

	string Description { get; set; }

	int Damage { get; set; }

	int MaxBulletCountInGun { get; set; }

	BindableProperty<int> BulletInGun { get; set; }

	BindableProperty<GunState> gunState { get; set; }
}

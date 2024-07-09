


// GunConfigModel
using System;
using QFramework;

[Serializable]
public class GunConfigModel : AbstractModel, IGunConfigModel, IModel, IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
{
	public int ID { get; set; }

	public string Name { get; set; }

	public BindableProperty<int> BulletCount { get; set; }

	public float CD { get; set; }

	public string Description { get; set; }

	public int Damage { get; set; }

	public int MaxBulletCountInGun { get; set; }

	public BindableProperty<int> BulletInGun { get; set; }

	public BindableProperty<GunState> gunState { get; set; }

	public GunConfigModel()
	{
	}

	public GunConfigModel(int id, string name, int bulletCount, float cd, string description, int damage, int bulletInGun, int maxBulletCountInGun)
	{
		ID = id;
		Name = name;
		BulletCount = new BindableProperty<int>(0)
		{
			Value = bulletCount
		};
		CD = cd;
		Description = description;
		Damage = damage;
		BulletInGun = new BindableProperty<int>(0)
		{
			Value = bulletInGun
		};
		gunState = new BindableProperty<GunState>(GunState.Idle)
		{
			Value = GunState.Idle
		};
		MaxBulletCountInGun = maxBulletCountInGun;
	}

	protected override void OnInit()
	{
	}
}

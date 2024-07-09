using System;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

[Serializable]
public class GunSystem : AbstractSystem, IGunSystem, ISystem, IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetUtility, ICanRegisterEvent, ICanSendEvent
{
	private int id;

	public IGunConfigModel CurrentGun { get; set; }

	public List<IGunConfigModel> GunList { get; set; }

	public GunSystem(int id)
	{
		this.id = id;
	}

	protected override void OnInit()
	{
		GunList = new List<IGunConfigModel>();
		CurrentGun = DataManager.Instance.GetWeaponInfoByID(id);
		CurrentGun.BulletInGun.Register(delegate
		{
			GameUIManager.Instance.UpdateBulletSlider();
		});
	}

	public void PickGun(int id)
	{
		if (CurrentGun.ID == id)
		{
			CurrentGun.BulletCount.Value += CurrentGun.MaxBulletCountInGun;
		}
		else if (GunList.Any((IGunConfigModel info) => info.ID == id))
		{
			IGunConfigModel gunInfo = GunList.FirstOrDefault((IGunConfigModel info) => info.ID == id);
			if (gunInfo != null)
			{
				gunInfo.BulletCount.Value += DataManager.Instance.GetWeaponInfoByID(id).MaxBulletCountInGun;
			}
		}
		else
		{
			GunConfigModel currentGunInfo = new GunConfigModel
			{
				ID = CurrentGun.ID,
				Name = CurrentGun.Name,
				BulletCount = new BindableProperty<int>(0)
				{
					Value = CurrentGun.BulletCount.Value
				},
				CD = CurrentGun.CD,
				Description = CurrentGun.Description,
				Damage = CurrentGun.Damage,
				BulletInGun = new BindableProperty<int>(0)
				{
					Value = CurrentGun.BulletInGun
				},
				gunState = new BindableProperty<GunState>(GunState.Idle)
				{
					Value = CurrentGun.gunState.Value
				},
				MaxBulletCountInGun = CurrentGun.MaxBulletCountInGun
			};
			GunList.Add(currentGunInfo);
			IGunConfigModel pickGunInfo = DataManager.Instance.GetWeaponInfoByID(id);
			GunConfigModel pickGun = new GunConfigModel
			{
				ID = pickGunInfo.ID,
				Name = pickGunInfo.Name,
				BulletCount = new BindableProperty<int>(0)
				{
					Value = pickGunInfo.BulletCount.Value
				},
				CD = pickGunInfo.CD,
				Description = pickGunInfo.Description,
				Damage = pickGunInfo.Damage,
				BulletInGun = new BindableProperty<int>(0)
				{
					Value = pickGunInfo.BulletInGun
				},
				gunState = new BindableProperty<GunState>(GunState.Idle)
				{
					Value = pickGunInfo.gunState.Value
				},
				MaxBulletCountInGun = pickGunInfo.MaxBulletCountInGun
			};
			CurrentGun = pickGun;
		}
		CurrentGun.BulletInGun.Register(delegate
		{
			GameUIManager.Instance.UpdateBulletSlider();
		});
	}

	public void ShiftGun(int id)
	{
		if (GunList.Count <= 0)
		{
			return;
		}
		GunConfigModel currentGunInfo = new GunConfigModel
		{
			ID = CurrentGun.ID,
			Name = CurrentGun.Name,
			BulletCount = new BindableProperty<int>(0)
			{
				Value = CurrentGun.BulletCount.Value
			},
			CD = CurrentGun.CD,
			Description = CurrentGun.Description,
			Damage = CurrentGun.Damage,
			BulletInGun = new BindableProperty<int>(0)
			{
				Value = CurrentGun.BulletInGun
			},
			gunState = new BindableProperty<GunState>(GunState.Idle)
			{
				Value = CurrentGun.gunState.Value
			},
			MaxBulletCountInGun = CurrentGun.MaxBulletCountInGun
		};
		GunList[currentGunInfo.ID - 1] = currentGunInfo;
		IGunConfigModel nextGunInfo = GunList[id - 1];
		GunConfigModel nextGun = new GunConfigModel
		{
			ID = nextGunInfo.ID,
			Name = nextGunInfo.Name,
			BulletCount = new BindableProperty<int>(0)
			{
				Value = nextGunInfo.BulletCount.Value
			},
			CD = nextGunInfo.CD,
			Description = nextGunInfo.Description,
			Damage = nextGunInfo.Damage,
			BulletInGun = new BindableProperty<int>(0)
			{
				Value = nextGunInfo.BulletInGun
			},
			gunState = new BindableProperty<GunState>(GunState.Idle)
			{
				Value = nextGunInfo.gunState.Value
			},
			MaxBulletCountInGun = nextGunInfo.MaxBulletCountInGun
		};
		if (nextGun != null)
		{
			CurrentGun = nextGun;
			if (nextGun.Name == "Pistol")
			{
				GameManager.Instance.PlayPistolReloadAudio();
			}
			else if (nextGun.Name == "Shotgun")
			{
				GameManager.Instance.PlayShotGunAudio();
			}
			CurrentGun.BulletInGun.Register(delegate
			{
				GameUIManager.Instance.UpdateBulletSlider();
			});
			CurrentGun.gunState.Value = GunState.Idle;
			Debug.Log(CurrentGun.Name);
		}
		else
		{
			Debug.LogError("NotExit!!!");
		}
	}

	public void ResetGun()
	{
		GunList.Clear();
		IGunConfigModel pistolInfo = DataManager.Instance.GetWeaponInfoByID(1);
		GunConfigModel pistol = new GunConfigModel
		{
			ID = pistolInfo.ID,
			Name = pistolInfo.Name,
			BulletCount = new BindableProperty<int>(0)
			{
				Value = pistolInfo.BulletCount.Value
			},
			CD = pistolInfo.CD,
			Description = pistolInfo.Description,
			Damage = pistolInfo.Damage,
			BulletInGun = new BindableProperty<int>(0)
			{
				Value = pistolInfo.BulletInGun
			},
			gunState = new BindableProperty<GunState>(GunState.Idle)
			{
				Value = pistolInfo.gunState.Value
			},
			MaxBulletCountInGun = pistolInfo.MaxBulletCountInGun
		};
		GunList.Add(pistol);
		CurrentGun = pistol;
		CurrentGun.BulletInGun.Register(delegate
		{
			GameUIManager.Instance.UpdateBulletSlider();
		});
	}
}

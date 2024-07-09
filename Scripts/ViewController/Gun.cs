using System;
using QFramework;
using UnityEngine;

public class Gun : MonoBehaviour, IController
{
	private GameObject bulletPre;

	public int BuffShot = 3;

	public float angles;

	private Transform playerTrans;

	private GunConfigModel currentGun => (GunConfigModel)this.GetSystem<IGunSystem>().CurrentGun;

	private void Awake()
	{
		if (playerTrans == null)
		{
			playerTrans = GameObject.FindWithTag("Player").transform;
		}
		bulletPre = Resources.Load<GameObject>("Bullet/PlayerBullet");
	}

	private void Start()
	{
		if (bulletPre != null)
		{
			PoolManager.Instance.InitPool(bulletPre, 10);
			bulletPre.transform.position = transform.position;
		}
	}

	private void Update()
	{
	}

	public void Attack()
	{
		if (currentGun.BulletInGun.Value <= 0 || currentGun.gunState.Value != 0)
		{
			return;
		}
		this.SendCommand(ShootingCommand.Single);
		GameManager.Instance.PlayerShootAudio();
		Quaternion localRotation;
		if (currentGun.ID == 1)
		{
			GameObject bulletClo = PoolManager.Instance.GetInstance<GameObject>(bulletPre);
			bulletClo.transform.position = transform.position;
			bulletClo.transform.rotation = Quaternion.Euler(0f, transform.rotation.y, 0f);
			bulletClo.SetActive(true);
			localRotation = playerTrans.localRotation;
			float targetDirection = localRotation.eulerAngles.y;
			Vector3 dirction = new Vector2(Mathf.Cos(targetDirection * ((float)Math.PI / 180f)), Mathf.Sin(targetDirection * ((float)Math.PI / 180f)));
			bulletClo.GetComponent<MagicBullet>().BulletLaunch(dirction);
		}
		else if (currentGun.ID == 2)
		{
			for (int i = -1; i < 2; i++)
			{
				float angleOffset = (float)i * 8f;
				GameObject bulletClo = PoolManager.Instance.GetInstance<GameObject>(bulletPre);
				bulletClo.transform.position = transform.position;
				localRotation = transform.localRotation;
				float direction = localRotation.eulerAngles.z + angleOffset;
				bulletClo.SetActive(true);
				Vector3 dirction = new Vector2(Mathf.Cos(direction * ((float)Math.PI / 180f)), Mathf.Sin(direction * ((float)Math.PI / 180f)));
				bulletClo.GetComponent<MagicBullet>().BulletLaunch(new Vector2(dirction.x * JudgeLaunchDir(), dirction.y));
			}
		}
	}

	private float JudgeLaunchDir()
	{
		Quaternion localRotation = playerTrans.localRotation;
		return (!(localRotation.eulerAngles.y > 90f)) ? 1 : (-1);
	}

	public void Reload()
	{
		if (currentGun.gunState.Value == GunState.Idle && currentGun.BulletInGun.Value != currentGun.MaxBulletCountInGun && (int)currentGun.BulletCount > 0)
		{
			Debug.Log(currentGun.CD);
			Debug.Log(currentGun.BulletInGun);
			Debug.Log(currentGun.BulletCount);
			this.SendCommand<ReloadCommand>();
		}
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}
}

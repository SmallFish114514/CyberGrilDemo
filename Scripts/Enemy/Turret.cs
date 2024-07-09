using System;
using QFramework;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Turret : MonoBehaviour, IController
{
	private GameObject enemyExplosionEffext;

	public IEnemyModel turrentModel;

	private GameObject dropItem_BulletGo;

	private void Awake()
	{
		enemyExplosionEffext = Resources.Load<GameObject>("ExplosionEffect");
		dropItem_BulletGo = Resources.Load<GameObject>("DropItem/DropItem_Ammo");
	}

	private void Start()
	{
		turrentModel = DataManager.Instance.GetEnemyInfoByID(2);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((collision.transform).CompareTag("Bullet"))
		{
			GameManager.Instance.PlayBulletHitAudio();
			this.SendCommand(new AttackEnemyCommand(2, this.GetSystem<IGunSystem>().CurrentGun.Damage));
			Die();
		}
	}

	private void Die()
	{
		if ((int)turrentModel.HP <= 0)
		{
			this.SendCommand<KillEnemyCommand>();
			float dropChance = Random.Range(0, 11);
			Instantiate(dropItem_BulletGo, transform.position, Quaternion.identity);
			turrentModel.ReSetHP();
			GameManager.Instance.EnemyExplosionAudio();
			GameObject explosionEffect = Instantiate(enemyExplosionEffext, transform.position, Quaternion.identity);
			Destroy(gameObject);
			Destroy(explosionEffect, 1f);
		}
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}
}

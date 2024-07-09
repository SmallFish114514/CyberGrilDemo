using System;
using QFramework;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Enemy_Drone : MonoBehaviour, IController
{
	public Player player;

	private GameObject enemyExplosionEffext;

	private Rigidbody2D rb;

	public GameObject damageNumTextPre;

	public IEnemyModel droneModel;

	private GameObject dropItem_HPGo;

	public bool isMoveingRight;

	private void Awake()
	{
		dropItem_HPGo = Resources.Load<GameObject>("DropItem/DropItem_Medical");
		enemyExplosionEffext = Resources.Load<GameObject>("ExplosionEffect");
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		isMoveingRight = true;
		droneModel = DataManager.Instance.GetEnemyInfoByID(1);
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		rb.velocity = (isMoveingRight ? Vector2.right : Vector2.left) * droneModel.MoveSpeed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((collision.transform).CompareTag("Ground") || (collision.transform).CompareTag("Building") || (collision.transform).CompareTag("Enemy"))
		{
			isMoveingRight = !isMoveingRight;
		}
		if (collision.gameObject.CompareTag("Player"))
		{
			this.SendCommand(new AttackPlayerCommand(droneModel.Damage));
			GameManager.Instance.PlayerShootAudio();
			GameObject explosionEffectClo = Instantiate(enemyExplosionEffext, transform.position, Quaternion.identity);
			GameManager.Instance.EnemyExplosionAudio();
			Destroy(explosionEffectClo, 1f);
			Destroy(gameObject);
		}
		if ((collision.transform).CompareTag("Bullet"))
		{
			GameManager.Instance.PlayBulletHitAudio();
			this.SendCommand(new AttackEnemyCommand(1, this.GetSystem<IGunSystem>().CurrentGun.Damage));
			SpawnDamageNum(droneModel.Damage, transform.position);
			Die();
		}
	}

	private void Die()
	{
		if ((int)droneModel.HP <= 0)
		{
			this.SendCommand<KillEnemyCommand>();
			float dropChance = Random.Range(0, 11);
			if (dropChance <= droneModel.ItemDropChance * 10f)
			{
				Instantiate(dropItem_HPGo, transform.position, Quaternion.identity);
			}
			droneModel.ReSetHP();
			GameManager.Instance.EnemyExplosionAudio();
			GameObject explosionEffect = Instantiate(enemyExplosionEffext, transform.position, Quaternion.identity);
			Destroy(gameObject);
			Destroy(explosionEffect, 1f);
		}
	}

	private void SpawnDamageNum(int damageNumCount, Vector2 spawnPos)
	{
		GameObject damageNumGo = Instantiate(damageNumTextPre, spawnPos, Quaternion.identity);
		DamageNum damageNum = damageNumGo.GetComponent<DamageNum>();
		if (damageNum != null)
		{
			damageNum.SetDamage(damageNumCount);
		}
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}
}

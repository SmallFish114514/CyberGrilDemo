


// TurretBullet
using QFramework;
using UnityEngine;

public class TurretBullet : MonoBehaviour, IController
{
	private float lifeTime;

	public GameObject ShotHit;

	private IEnemyModel turretModel;

	private Rigidbody2D rb;

	private void Awake()
	{
		rb = transform.GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		turretModel = DataManager.Instance.GetEnemyInfoByID(2);
		lifeTime = 2f;
		Invoke("ReSetBullet", lifeTime);
	}

	private void ReSetBullet()
	{
		gameObject.SetActive(false);
		rb.velocity = Vector2.zero;
		gameObject.transform.rotation = Quaternion.identity;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((collision.transform).CompareTag("Player"))
		{
			this.SendCommand(new AttackPlayerCommand(turretModel.Damage));
			InstantiateShotHit();
		}
		else if ((collision.transform).CompareTag("Ground"))
		{
			InstantiateShotHit();
		}
	}

	private void InstantiateShotHit()
	{
		GameObject ShotHitEffect = Instantiate(ShotHit, transform.position, Quaternion.identity);
		Destroy(ShotHitEffect, 0.5f);
		gameObject.SetActive(false);
		rb.velocity = Vector2.zero;
		gameObject.transform.rotation = Quaternion.identity;
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}
}

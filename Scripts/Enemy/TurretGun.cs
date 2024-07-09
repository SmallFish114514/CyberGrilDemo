using UnityEngine;

public class TurretGun : MonoBehaviour
{
	private GameObject turretBullet;

	public LayerMask PlayerLayer;

	private float attackRange = 5f;

	public float Shootinterval = 1f;

	private float currentTimeInterval;

	public float speed = 5f;

	private void Awake()
	{
		turretBullet = Resources.Load<GameObject>("Bullet/EnemyBullet");
	}

	private void Start()
	{
		currentTimeInterval = Shootinterval;
		if (turretBullet != null)
		{
			PoolManager.Instance.InitPool(turretBullet, 10);
			turretBullet.transform.position = transform.position;
		}
	}

	private void Update()
	{
		currentTimeInterval -= Time.deltaTime;
		if (DetectPlayer(Vector2.left) || DetectPlayer(Vector2.right))
		{
			Launch();
		}
	}

	private bool DetectPlayer(Vector2 dirction)
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, dirction, attackRange, PlayerLayer);
		return hit.collider != null && hit.collider.CompareTag("Player");
	}

	private void Launch()
	{
		if (!(currentTimeInterval <= 0f))
		{
			return;
		}
		GameObject enemyBulletPre = PoolManager.Instance.GetInstance<GameObject>(turretBullet);
		if (enemyBulletPre != null)
		{
			GameManager.Instance.PlayTurretShootAudio();
			enemyBulletPre.transform.position = transform.position;
			enemyBulletPre.transform.rotation = Quaternion.Euler(0f, transform.rotation.y, 0f);
			Rigidbody2D rb = enemyBulletPre.GetComponent<Rigidbody2D>();
			if (rb != null)
			{
				Vector2 dirction = (DetectPlayer(Vector2.left) ? Vector2.left : Vector2.right);
				rb.velocity = dirction * speed;
			}
			else
			{
				Debug.LogError("Rigidbody2D component missing on enemy bullet prefab.");
			}
		}
		else
		{
			Debug.LogError("Failed to get enemy bullet from pool.");
		}
		currentTimeInterval = Shootinterval;
	}
}

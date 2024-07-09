using QFramework;
using UnityEngine;

public class MagicBullet : MonoBehaviour, IController
{
	private IGunSystem gunSystem;

	public GameObject ShotHit;

	private float lifeTime;

	private float speed;

	private Rigidbody2D rb;

	private void Awake()
	{
		rb = transform.GetComponent<Rigidbody2D>();
		gunSystem = this.GetSystem<IGunSystem>();
	}

	private void OnEnable()
	{
		speed = 7f;
		lifeTime = 2f;
		Invoke("ReSetBullet", lifeTime);
	}

	private void ReSetBullet()
	{
		gameObject.SetActive(false);
		rb.velocity = Vector2.zero;
		gameObject.transform.rotation = Quaternion.identity;
	}

	public void BulletLaunch(Vector3 direction)
	{
		if (Mathf.Abs(transform.GetComponent<Rigidbody2D>().velocity.x) <= 0.1f)
		{
			rb.velocity = direction * speed;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((collision.transform).CompareTag("Enemy") || (collision.transform).CompareTag("Building") || (collision.transform).CompareTag("Ground"))
		{
			GameObject ShotHiteEffect = Instantiate(ShotHit, transform.position, Quaternion.identity);
			Destroy(ShotHiteEffect, 0.5f);
			gameObject.SetActive(false);
		}
		rb.velocity = Vector2.zero;
		gameObject.transform.rotation = Quaternion.identity;
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}
}

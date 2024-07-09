using TMPro;
using UnityEngine;

public class DamageNum : MonoBehaviour
{
	public TextMeshPro damageNumText;

	private float liftTime = 0.5f;

	public float speed = 2f;

	private void Start()
	{
		Destroy(gameObject, liftTime);
	}

	private void Update()
	{
		transform.Translate(Vector2.up * speed * Time.deltaTime);
	}

	public void SetDamage(int damageNum)
	{
		damageNumText.text = damageNum.ToString();
	}
}

using QFramework;
using UnityEngine;

public class DropItem_HP : MonoBehaviour
{
	private bool isPickUp;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (((collision).transform).CompareTag("Player") && !isPickUp)
		{
			GameManager.Instance.PlayPickUpItemAudio();
			Inventory.Instance.StoreItem(1);
			isPickUp = true;
			Destroy(gameObject);
		}
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}
}

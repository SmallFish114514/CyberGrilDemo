using QFramework;
using UnityEngine;

public class DropItem_Ammo : MonoBehaviour, IController
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (((collision).transform).CompareTag("Player"))
		{
			GameManager.Instance.PlayPickUpItemAudio();
			Inventory.Instance.StoreItem(2);
			Destroy(gameObject);
		}
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}
}

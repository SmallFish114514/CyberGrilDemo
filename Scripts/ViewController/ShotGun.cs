


// ShotGun
using QFramework;
using UnityEngine;

public class ShotGun : MonoBehaviour, IController, IBelongToArchitecture, ICanSendCommand, ICanSendQuery, ICanGetSystem, ICanGetModel, ICanRegisterEvent
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (((collision).transform).CompareTag("Player"))
		{
			GameManager.Instance.PlayShotGunAudio();
			UIWeaponItemManager.Instance.StoreWeaponItem("ShotGun");
			this.SendCommand(new PickGunCommand(2));
			Destroy(gameObject);
		}
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}
}

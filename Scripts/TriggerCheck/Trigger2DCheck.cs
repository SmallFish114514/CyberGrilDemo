using UnityEngine;

public class Trigger2DCheck : MonoBehaviour
{
	public LayerMask TargetLayer;

	public int TriggerCount;

	public bool Triggered => TriggerCount > 0;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (IsInLayerMask(collision.gameObject, TargetLayer) || collision.transform.CompareTag("Player"))
		{
			TriggerCount++;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (IsInLayerMask(collision.gameObject, TargetLayer) || collision.transform.CompareTag("Player"))
		{
			TriggerCount--;
		}
	}

	private bool IsInLayerMask(GameObject obj, LayerMask mask)
	{
		int objLayerMask = 1 << obj.layer;
		return (mask.value & objLayerMask) > 0;
	}
}

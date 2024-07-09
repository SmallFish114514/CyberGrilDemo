using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Transform playerTrans;

	private void LateUpdate()
	{
		if (playerTrans == null)
		{
			GameObject playerGo = GameObject.FindWithTag("Player");
			if (playerGo == null)
			{
				return;
			}
			playerTrans = playerGo.transform;
		}
		Vector3 cameraPos = transform.position;
		ShortcutExtensions.DOMove(transform, new Vector3(playerTrans.position.x, playerTrans.position.y + 1.2f, -10f), 0.5f, false);
		transform.position = cameraPos;
	}
}

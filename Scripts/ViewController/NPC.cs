using UnityEngine;

public class NPC : MonoBehaviour
{
	private Trigger2DCheck dialogueTrigger;

	private bool isTrigger;

	private GameObject interactImg;

	public bool isShowInteract;

	public Canvas InteractUI;

	private void Awake()
	{
		dialogueTrigger = (transform.Find("CheckPoint")).GetComponent<Trigger2DCheck>();
		InteractUI.gameObject.SetActive(false);
		if (InteractUI != null)
		{
			interactImg = (InteractUI.transform.Find("bg")).gameObject;
		}
	}

	private void Update()
	{
		InteractUIShow();
	}

	private void InteractUIShow()
	{
		isTrigger = dialogueTrigger.Triggered;
		if (isTrigger)
		{
			Vector3 worldPos = transform.position + new Vector3(0f, 0.35f, 0f);
			Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
			RectTransformUtility.ScreenPointToLocalPointInRectangle((InteractUI).GetComponent<RectTransform>(), screenPos, (Camera)null, out Vector2 localPoint);
			interactImg.transform.localPosition =localPoint;
			InteractUI.gameObject.SetActive(true);
			isShowInteract = true;
		}
		else
		{
			(InteractUI).gameObject.SetActive(false);
			isShowInteract = false;
		}
	}
}

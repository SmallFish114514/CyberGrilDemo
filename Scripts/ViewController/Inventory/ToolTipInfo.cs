using UnityEngine;
using UnityEngine.UI;

public class ToolTipInfo : MonoBehaviour
{
	public float SmoothingValue = 5f;

	private Text toolTipText;

	private Text contentText;

	private CanvasGroup canvasGroup;

	private float targetAlpha = 1f;

	private void Awake()
	{
		toolTipText = transform.GetComponent<Text>();
		canvasGroup = transform.GetComponent<CanvasGroup>();
		contentText = (transform.Find("Content")).GetComponent<Text>();
	}

	private void Update()
	{
		if (canvasGroup.alpha != targetAlpha)
		{
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, SmoothingValue * Time.deltaTime);
			if (Mathf.Abs(canvasGroup.alpha - targetAlpha) <= 0.01f)
			{
				canvasGroup.alpha = targetAlpha;
			}
		}
	}

	public void ShowToolTip(string content)
	{
		toolTipText.text = content;
		contentText.text = content;
		targetAlpha = 1f;
	}

	public void HideToolTip()
	{
		targetAlpha = 0f;
	}

	public void SetPos(Vector3 pos)
	{
		transform.localPosition = pos;
	}
}

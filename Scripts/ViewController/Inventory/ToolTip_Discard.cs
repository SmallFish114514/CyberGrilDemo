using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToolTip_Discard : MonoBehaviour
{
	public Button ConfirmButton;

	public Button CancelButton;

	public Slider DiscardAmountSlider;

	public Text DisplayAmountText;

	private void Start()
	{
		(CancelButton.onClick).AddListener(() =>
		{
			gameObject.SetActive(false);
			Inventory.Instance.ShowPickUpItem();
		});
	}

	private void Update()
	{
		DisplayAmountText.text = "丢弃数量：" + DiscardAmountSlider.value;
	}

	public int GetCustomDiscardAmount()
	{
		return (int)DiscardAmountSlider.value;
	}
}

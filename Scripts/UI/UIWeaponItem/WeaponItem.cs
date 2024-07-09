using UnityEngine;
using UnityEngine.UI;

public class WeaponItem : MonoBehaviour
{
	private Image gunItemIcon;

	public void SetGunItem(string mName)
	{
		gunItemIcon = GetComponent<Image>();
		Sprite weaponItemImg = Resources.Load<Sprite>("Weapon/" + mName);
		if (weaponItemImg != null)
		{
			gunItemIcon.sprite = weaponItemImg;
		}
	}
}

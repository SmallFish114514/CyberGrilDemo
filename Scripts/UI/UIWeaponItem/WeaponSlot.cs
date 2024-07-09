using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
	private GameObject WeaponItemGo;

	public string mName;

	private void Awake()
	{
		WeaponItemGo = Resources.Load<GameObject>("Weapon/WeaponItem");
	}

	public void StoreItem(string name)
	{
		if (transform.childCount == 0)
		{
			mName = name;
			GameObject itemClo = Instantiate(WeaponItemGo);
			itemClo.transform.SetParent(transform);
			itemClo.transform.position = transform.position;
			itemClo.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 45f));
			itemClo.transform.localScale = Vector3.one;
			itemClo.GetComponent<WeaponItem>().SetGunItem(name);
		}
	}
}

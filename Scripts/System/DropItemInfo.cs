using System;

[Serializable]
public class DropItemInfo
{
	public string Name { get; set; }

	public int Id { get; set; }

	public int AddAmmoValue { get; set; }

	public int AddHpValue { get; set; }

	public string Description { get; set; }

	public string AssetPath { get; set; }

	public DropItemInfo(string name, int id, int addAmmo, int addHp, string assetPath, string description)
	{
		Name = name;
		Id = id;
		AddAmmoValue = addAmmo;
		AddHpValue = addHp;
		AssetPath = assetPath;
		Description = description;
	}
}

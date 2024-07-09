


// PlayerModel
using System;
using QFramework;

[Serializable]
public class PlayerModel : AbstractModel, IPlayerModel, IModel, IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
{
	private const int maxHp = 100;

	public static readonly float SprintSpeedMultiplier = 1.5f;

	public string Name => "小鱼干牛批";

	public int MaxHp => 100;

	public BindableProperty<int> HP { get; } = new BindableProperty<int>(0)
	{
		Value = 100
	};


	public int JumpForce => 5;

	public float MoveSpeed => 2.5f;

	public BindableProperty<float> ActualSpeed { get; } = new BindableProperty<float>(0f)
	{
		Value = 2.5f
	};


	float IPlayerModel.SprintSpeedMultiplier => 1.5f;

	protected override void OnInit()
	{
		HP.Register(delegate
		{
			GameUIManager.Instance.UpdateHpSlider();
		});
	}
}

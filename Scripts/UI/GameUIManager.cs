using System.IO;
using QFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour, IController
{
	private static GameUIManager instance;

	private IStateSystem stateSystem;

	private IPlayerModel playerModel;

	public Slider HpSlider;

	public Slider SpSlider;

	public Button SettingButton;

	private Canvas settingPanel;

	private CanvasGroup canvasGroup;

	[HideInInspector]
	public DialogueManager DialoguePanel;

	public GameObject OptionPanel;

	private GameObject inventoryPanel;

	public Text KillCountText;

	[HideInInspector]
	public string streamPath;

	public static GameUIManager Instance => instance;

	private GunConfigModel currentGun => (GunConfigModel)this.GetSystem<IGunSystem>().CurrentGun;

	public GameObject InventoryPanel
	{
		get
		{
			if (inventoryPanel == null)
			{
				inventoryPanel = (Object.FindObjectOfType<InventoryManager>()).gameObject;
			}
			return inventoryPanel;
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
		stateSystem = this.GetSystem<IStateSystem>();
		playerModel = this.GetModel<IPlayerModel>();
		canvasGroup = GetComponent<CanvasGroup>();
		DialoguePanel = (transform.Find("BG/DialoguePanel")).GetComponent<DialogueManager>();
		GameObject setting_Panel = GameObject.FindGameObjectWithTag("GameSettingPanel");
		if (setting_Panel != null)
		{
			settingPanel = setting_Panel.GetComponent<Canvas>();
		}
		else
		{
			Debug.LogError(" SettingPanel is NUll");
		}
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void Start()
	{
		InventoryPanel.SetActive(false);
		if (DialoguePanel != null)
		{
			DialoguePanel.gameObject.SetActive(false);
		}
		settingPanel.gameObject.SetActive(false);
		SettingButton.onClick.AddListener(() =>
		{
			GameManager.Instance.PlayClikAudio();
			settingPanel.gameObject.SetActive(true);
		});
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name != "GameMain")
		{
			HideUI();
		}
		else
		{
			ShowUI();
		}
	}

	private void HideUI()
	{
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = false;
		settingPanel.gameObject.SetActive(false);
	}

	private void ShowUI()
	{
		canvasGroup.alpha = 1f;
		canvasGroup.blocksRaycasts = true;
	}

	public void SaveGame()
	{
		ISaveDataModel save = DataManager.Instance.SaveGame();
		streamPath = Application.streamingAssetsPath + "/ByJson.json";
		string saveByJson = JsonUtility.ToJson(save);
		StreamWriter streamWriter = new StreamWriter(streamPath);
		streamWriter.Write(saveByJson);
		streamWriter.Close();
	}

	public void UpdateHpSlider()
	{
		HpSlider.value = (float)playerModel.HP.Value / (float)playerModel.MaxHp;
	}

	public void UpdateBulletSlider()
	{
		SpSlider.value = (float)currentGun.BulletInGun.Value / (float)currentGun.MaxBulletCountInGun;
	}

	public void UpdateKillEnemyCount()
	{
		KillCountText.text = "击杀数量:" + stateSystem.KillCount.Value;
	}

	public void InventorySwitch()
	{
		InventoryPanel.SetActive(!InventoryPanel.activeSelf);
	}

	public void SettingPanelSwitch()
	{
		settingPanel.gameObject.SetActive(!settingPanel.gameObject.activeSelf);
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}

	private void OnDestroy()
	{
		stateSystem = null;
		playerModel = null;
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}

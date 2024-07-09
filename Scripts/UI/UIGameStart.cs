using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameStart : MonoBehaviour
{
	public Button NewGameStartButton;

	public Button ContinueGameButton;

	public Button SettingGButton;

	public Button ExitGameButton;

	public GameObject SettingCnavas;

	private Button closeSettingButton;

	private Slider VolumeSlider;

	private AudioSource audioSource;

	public AudioClip GameStartBgClip;

	public AudioClip ClikButtonClip;

	public float VolumeScale = 0.5f;

	[Obsolete("FindChild已过时", false)]
	private void Awake()
	{
		SettingCnavas.SetActive(false);
		NewGameStartButton.onClick.AddListener(() =>
		{
			PlayClikAudio();
			DeleteJson();
			SceneManager.LoadScene("GameMain");
		});
		ContinueGameButton.onClick.AddListener(() =>
		{
			PlayClikAudio();
			SceneManager.LoadScene("GameMain");
		});
		SettingGButton.onClick.AddListener(() =>
		{
			PlayClikAudio();
			SettingCnavas.gameObject.SetActive(true);
		});
		ExitGameButton.onClick.AddListener(() =>
		{
			PlayClikAudio();
			Application.Quit();
		});
		if (SettingCnavas != null)
		{
			closeSettingButton = SettingCnavas.transform.FindChild("ClosePanelButton").GetComponent<Button>();
			VolumeSlider = SettingCnavas.transform.FindChild("VolumeSlider").GetComponent<Slider>();
		}
		audioSource = transform.GetComponent<AudioSource>();
	}

	private void Start()
	{
		PlayGameStartAudio();
		closeSettingButton.onClick.AddListener(() =>
		{
			PlayClikAudio();
			SettingCnavas.gameObject.SetActive(false);
		});
	}

	private void Update()
	{
		audioSource.volume = VolumeSlider.value;
	}

	private void PlayGameStartAudio()
	{
		audioSource.PlayOneShot(GameStartBgClip, VolumeScale);
	}

	private void PlayClikAudio()
	{
		audioSource.PlayOneShot(ClikButtonClip, VolumeScale);
	}

	private void DeleteJson()
	{
		string streamPath = Application.streamingAssetsPath + "/ByJson.json";
		if (File.Exists(streamPath))
		{
			try
			{
				File.Delete(streamPath);
				Debug.Log(("文件已成功删除：" + streamPath));
				return;
			}
			catch (IOException e)
			{
				Debug.LogError(("删除文件时发生错误：" + e.Message));
				return;
			}
		}
		Debug.LogWarning(("文件不存在：" + streamPath));
	}
}

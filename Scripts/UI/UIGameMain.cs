using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameMain : MonoBehaviour
{
	private static UIGameMain instance;

	public Button SaveGameButton;

	public Button ReloadGameButton;

	public Button ReturnGamePanel;

	public Button ReturnMainPanel;

	public Slider VolumeSlider;

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		SaveGameButton.onClick.AddListener(() =>
		{
			GameManager.Instance.PlayClikAudio();
			GameUIManager.Instance.SaveGame();
		});

		ReloadGameButton.onClick.AddListener(() =>
		{
			GameManager.Instance.PlayClikAudio();
			DeleteJson();
			SceneManager.LoadScene(1);
		});
		ReturnGamePanel.onClick.AddListener(() =>
		{
			GameManager.Instance.PlayClikAudio();
			Time.timeScale = 1f;
			transform.gameObject.SetActive(false);
		});
		ReturnMainPanel.onClick.AddListener(() =>
		{
			GameManager.Instance.PlayClikAudio();
			transform.gameObject.SetActive(false);
			((InventoryManager.instance).transform).gameObject.SetActive(false);
			SceneManager.LoadScene(0);
		});
	}

	private void Update()
	{
		GameManager.Instance.VolumeScale = VolumeSlider.value;
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

	private IEnumerator LoadSceneAsync()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync("GameStart");
		while (!operation.isDone)
		{
			yield return null;
		}
	}
}

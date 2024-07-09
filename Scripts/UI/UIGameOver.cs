using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
	public Button RetryButton;

	public Button ReturnMenuButton;

	private AudioSource audioSource;

	public AudioClip ClikButtonClip;

	public AudioClip GameOverClip;

	public float VolumeScale = 0.5f;

	private void Awake()
	{
		audioSource = transform.GetComponent<AudioSource>();
		RetryButton.onClick.AddListener(() =>
		{
			PlayClipAudio();
			DeleteJson();
			Debug.Log("again");
			SceneManager.LoadScene(1);
		});
		ReturnMenuButton.onClick.AddListener(() =>
		{
			PlayClipAudio();
			Debug.Log("returnMenu");
			SceneManager.LoadScene(0);
		});
		audioSource.PlayOneShot(GameOverClip);
	}

	private void Start()
	{
		PlayGameOverAudio();
	}

	private void PlayGameOverAudio()
	{
		audioSource.PlayOneShot(GameOverClip, VolumeScale);
	}

	private void PlayClipAudio()
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

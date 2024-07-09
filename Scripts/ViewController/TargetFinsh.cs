using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetFinsh : MonoBehaviour, IController
{
	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (this.GetSystem<IStateSystem>().KillCount.Value >= 13)
		{
			SceneManager.LoadScene("GameOver");
		}
	}
}

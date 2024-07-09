using UnityEngine;

public class Instruction : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			gameObject.SetActive(false);
		}
	}
}

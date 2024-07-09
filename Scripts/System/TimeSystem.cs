using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class TimeSystem : AbstractSystem, ITimeSystem, ISystem
{
	public class TimeSystemUpdateMonobehaviour : MonoBehaviour
	{
		public event Action OnUpdate;

		private void Update()
		{
			this.OnUpdate?.Invoke();
		}
	}

	private LinkedList<DelayTask> delayTaskList = new LinkedList<DelayTask>();

	public float currentSeconds { get; private set; }

	public void AddDelayTask(float seconds, Action OnFinsh)
	{
		DelayTask delayTask = new DelayTask
		{
			Seconds = seconds,
			OnFinsh = OnFinsh,
			TaskState = DelayTaskState.NotStart
		};
		delayTaskList.AddLast(delayTask);
	}

	protected override void OnInit()
	{
		GameObject TimeSystemUpdateBehaviourObj = new GameObject("TimeSystemUpdateMonobehaviour");
        UnityEngine.Object.DontDestroyOnLoad(TimeSystemUpdateBehaviourObj);
		TimeSystemUpdateMonobehaviour updateBehaviour = TimeSystemUpdateBehaviourObj.AddComponent<TimeSystemUpdateMonobehaviour>();
		updateBehaviour.OnUpdate += OnUpdate;
	}

	public void OnUpdate()
	{
		currentSeconds += Time.deltaTime;
		if (delayTaskList.Count <= 0)
		{
			return;
		}
		LinkedListNode<DelayTask> currenNode = delayTaskList.First;
		while (currenNode != null)
		{
			LinkedListNode<DelayTask> nextNode = currenNode.Next;
			DelayTask currentTask = currenNode.Value;
			if (currentTask.TaskState == DelayTaskState.NotStart)
			{
				currentTask.TaskState = DelayTaskState.Started;
				currentTask.StartSeconds = currentSeconds;
				currentTask.FinshSeconds = currentSeconds + currentTask.Seconds;
			}
			else if (currentTask.TaskState == DelayTaskState.Started && currentSeconds >= currentTask.FinshSeconds)
			{
				currentTask.TaskState = DelayTaskState.Finsh;
				currentTask.OnFinsh();
				currentTask.OnFinsh = null;
				delayTaskList.Remove(currenNode);
			}
			currenNode = nextNode;
		}
	}
}

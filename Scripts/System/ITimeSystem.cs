using System;
using QFramework;

public interface ITimeSystem : ISystem
{
	float currentSeconds { get; }

	void AddDelayTask(float seconds, Action OnFinsh);
}

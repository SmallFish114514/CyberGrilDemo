using System;

public class DelayTask
{
	public float Seconds { get; set; }

	public Action OnFinsh { get; set; }

	public float StartSeconds { get; set; }

	public float FinshSeconds { get; set; }

	public DelayTaskState TaskState { get; set; }
}

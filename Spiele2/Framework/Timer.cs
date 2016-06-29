namespace Framework
{
	public class Timer : ITimedUpdate
	{
		public Timer(float interval)
		{
			this.Interval = interval;
			this.Count = 0;
			this.Enabled = false;
		}

		public uint Count { get; private set; }
		public float ElapsedTime { get { return elapsedTime; } }
		public bool Enabled { get; set; }
		public delegate void TimerElapsed(float absoluteTime);
		public event TimerElapsed OnTimerElapsed;
		public float Interval { get; set; }

		public void Update(float absoluteTime)
		{
			if (!Enabled)
			{
				lastElapsedTime = absoluteTime;
				elapsedTime = 0.0f;
				return;
			}
			elapsedTime = absoluteTime - lastElapsedTime;
			if (elapsedTime > Interval)
			{
				OnTimerElapsed?.Invoke(absoluteTime);
				lastElapsedTime = absoluteTime;
				elapsedTime = 0.0f;
				++Count;
			}
		}

		private float lastElapsedTime = 0.0f;
		private float elapsedTime = 0.0f;
	}
}

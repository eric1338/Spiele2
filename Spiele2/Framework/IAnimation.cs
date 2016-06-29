namespace Framework
{
	public interface IAnimation
	{
		float AnimationLength { get; set; }

		void Draw(AABR rectangle, float totalSeconds);
	}
}
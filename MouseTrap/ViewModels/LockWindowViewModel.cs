namespace MouseTrap.ViewModels
{
	public abstract class LockWindowViewModel : IViewModel
	{
		public string Title { get; protected set; } = string.Empty;
		public string ProcessPath { get; protected set; } = string.Empty;
		public bool WindowIsFocused { get; protected set; }
		public double WindowHeight { get; protected set; }
		public double WindowWidth { get; protected set; }
		public double LeftOffset { get; set; }
		public double RightOffset { get; set; }
		public double TopOffset { get; set; }
		public double BottomOffset { get; set; }
		public double[] BoundaryOffset { get => new double[] { LeftOffset, RightOffset, TopOffset, BottomOffset }; }
	}
}

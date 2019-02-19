namespace MouseTrap.ViewModels
{
	public abstract class LockWindowViewModel : IViewModel
	{
		public string Title { get; set; } = string.Empty;
		public string ProcessPath { get; set; } = string.Empty;
		public double WindowHeight { get; set; }
		public double WindowWidth { get; set; }
		public bool WindowIsFocused { get; set; }

		public double LeftOffset { get; set; }
		public double RightOffset { get; set; }
		public double TopOffset { get; set; }
		public double BottomOffset { get; set; }

		public double[] BoundaryOffset { get => new double[] { LeftOffset, RightOffset, TopOffset, BottomOffset }; }
	}
}

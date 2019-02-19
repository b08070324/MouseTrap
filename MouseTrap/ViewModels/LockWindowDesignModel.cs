namespace MouseTrap.ViewModels
{
	public class LockWindowDesignModel : LockWindowViewModel
	{
		public LockWindowDesignModel()
		{
			Title = "This is a test title";
			WindowHeight = 1280;
			WindowWidth = 1920;
			LeftOffset = RightOffset = TopOffset = BottomOffset = 8;
			WindowIsFocused = true;
		}
	}
}

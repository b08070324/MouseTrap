namespace MouseTrap.Models
{
	public class WindowModel : ModelWithNotification
	{
		// Window title for selected window
		private string title = "Select a program";
		public string Title { get => title; set => SetField(ref title, value); }

		// Full process path
		private string process = "";
		public string Process { get => process; set => SetField(ref process, value); }

		// Top coord for window
		private int top = 0;
		public int Top { get => top; set => SetField(ref top, value); }

		// Left coord for window
		private int left = 0;
		public int Left { get => left; set => SetField(ref left, value); }

		// (Right - Left) for selected window
		private int width = 0;
		public int Width { get => width; set => SetField(ref width, value); }

		// (Bottom - Top) for selected window
		private int height = 0;
		public int Height { get => height; set => SetField(ref height, value); }

		// Selected window currently has focus
		private bool hasFocus = false;
		public bool HasFocus { get => hasFocus; set => SetField(ref hasFocus, value); }

		// Trap button state
		private bool mouseTrapRequested = false;
		public bool MouseTrapRequested { get => mouseTrapRequested; set => SetField(ref mouseTrapRequested, value); }

		// Selected index for DataGrid
		private int selectedIndex = -1;
		public int SelectedIndex { get => selectedIndex; set => SetField(ref selectedIndex, value); }
	}
}

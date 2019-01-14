namespace MouseTrap.Models
{
	public class WindowModel : ModelWithNotification
	{
		private string title = "Select a program";
		private string process = "";
		private int top = 0;
		private int left = 0;
		private int width = 0;
		private int height = 0;
		private bool hasFocus = false;
		private int trapMargin = 8;
		private bool mouseTrapRequested = false;
		private int selectedIndex = -1;

		// Window title for selected window
		public string Title
		{
			get { return title; }
			set { SetField(ref title, value); }
		}

		// Full process path
		public string Process
		{
			get { return process; }
			set { SetField(ref process, value); }
		}

		// Top coord for window
		public int Top
		{
			get { return top; }
			set { SetField(ref top, value); }
		}

		// Left coord for window
		public int Left
		{
			get { return left; }
			set { SetField(ref left, value); }
		}

		// (Right - Left) for selected window
		public int Width
		{
			get { return width; }
			set { SetField(ref width, value); }
		}

		// (Bottom - Top) for selected window
		public int Height
		{
			get { return height; }
			set { SetField(ref height, value); }
		}

		// Selected window currently has focus
		public bool HasFocus
		{
			get { return hasFocus; }
			set { SetField(ref hasFocus, value); }
		}

		// Amount to reduce window area by, when constraining mouse
		public int TrapMargin
		{
			get { return trapMargin; }
			set { SetField(ref trapMargin, value); }
		}

		// Trap button state
		public bool MouseTrapRequested
		{
			get { return mouseTrapRequested; }
			set { SetField(ref mouseTrapRequested, value); }
		}

		// Selected index for DataGrid
		public int SelectedIndex
		{
			get { return selectedIndex; }
			set { SetField(ref selectedIndex, value); }
		}
	}
}

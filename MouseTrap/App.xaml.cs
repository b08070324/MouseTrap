using System.Collections.Generic;
using System.Windows;

using MouseTrap.Foundation;

namespace MouseTrap
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application, IMouseTrapApplication
	{
		private WindowManager windowManager = new WindowManager();
		private MouseHookManager mouseHookManager = new MouseHookManager();

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			mouseHookManager.HookMouse();
			MainWindow window = new MainWindow();
			window.Show();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			mouseHookManager.UnhookMouse();
			base.OnExit(e);
		}

		public ICollection<WindowItem> GetWindowList()
		{
			return windowManager.GetWindowList();
		}

		public UpdatedWindowDetails UpdateWindowDetails()
		{
			if (windowManager.UpdateWindowDetails())
			{
				mouseHookManager.SetRegion(windowManager.SelectedWindow.BoundingDimensions);

				return new UpdatedWindowDetails
				{
					Title = windowManager.SelectedWindow.Title,
					Process = windowManager.SelectedWindow.ProcessName,
					Top = windowManager.SelectedWindow.BoundingDimensions.Top,
					Left = windowManager.SelectedWindow.BoundingDimensions.Left,
					Width = (windowManager.SelectedWindow.BoundingDimensions.Right - windowManager.SelectedWindow.BoundingDimensions.Left),
					Height = (windowManager.SelectedWindow.BoundingDimensions.Bottom - windowManager.SelectedWindow.BoundingDimensions.Top),
					HasFocus = windowManager.SelectedWindow.IsInForeground
				};
			}

			return null;
		}

		public void SetMouseHookState(bool mouseTrapRequested)
		{
			mouseHookManager.TrapMouse = mouseTrapRequested && (windowManager.SelectedWindow != null && windowManager.SelectedWindow.IsInForeground);
		}

		public void SelectWindow(WindowItem windowItem)
		{
			windowManager.SelectWindow(windowItem);
		}

		public bool IsWindowSelected { get => (windowManager.SelectedWindow != null); }
	}
}

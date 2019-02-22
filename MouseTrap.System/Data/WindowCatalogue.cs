using MouseTrap.Interop;
using MouseTrap.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Automation;

namespace MouseTrap.Data
{
	public class WindowCatalogue : IWindowCatalogue
	{
		private int CurrentProcessId { get; }
		private Action<IWindow> Callback { get; set; }

		private static readonly string[] BlacklistedClassNames = new string[]
		{
			"Shell_TrayWnd",
			"Shell_SecondaryTrayWnd",
			"SysListView32",
			"Progman"
		};

		public WindowCatalogue()
		{
			CurrentProcessId = Process.GetCurrentProcess().Id;
		}

		public void EnumerateWindows(Action<IWindow> callback)
		{
			var rootChildren = AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition);
			foreach (AutomationElement element in rootChildren)
			{
				if (element.Current.ProcessId == CurrentProcessId) continue;
				if (BlacklistedClassNames.Contains(element.Current.ClassName)) continue;
				if (string.IsNullOrEmpty(element.Current.Name)) continue;

				var window = new EnumeratedWindow
				{
					Handle = new IntPtr(element.Current.NativeWindowHandle),
					ProcessId = (uint)element.Current.ProcessId,
					Title = element.Current.Name,
					Left = element.Current.BoundingRectangle.Left,
					Top = element.Current.BoundingRectangle.Top,
					Right = element.Current.BoundingRectangle.Right,
					Bottom = element.Current.BoundingRectangle.Bottom,
					IsMinimized = element.Current.IsOffscreen,
				};

				callback(window);
			}
		}
	}
}

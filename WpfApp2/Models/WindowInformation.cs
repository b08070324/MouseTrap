using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WpfApp2.Interop;

namespace WpfApp2.Models
{
    public class WindowInformation
	{
		public IntPtr Handle { get; set; }
		public string Name { get; set; }
		public uint ProcessId { get; set; }
		public string ProcessName { get; set; }
		public string FullProcessName { get; set; }
		public int Left { get; set; }
		public int Top { get; set; }
		public int Right { get; set; }
		public int Bottom { get; set; }
		public IntPtr ExStyle { get; set; }

		public WindowInformation()
		{
		}

		public WindowInformation(IntPtr hWnd)
		{
			PopulateMainData(hWnd);
			UpdateVariableData();
		}

		public void Update()
		{
			UpdateVariableData();
		}

		private void PopulateMainData(IntPtr hWnd)
		{
			if (hWnd == null || hWnd == IntPtr.Zero) return;

			// Window handle
			this.Handle = hWnd;

			// Thread process ID
			Win32Interop.GetWindowThreadProcessId(hWnd, out uint procId);
			this.ProcessId = procId;

			// Name of executable
			var process = Process.GetProcessById((int)procId);
			this.ProcessName = process.ProcessName;

			// Get limited info handle
			IntPtr limitedHandle = Win32Interop.OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, (int)procId);

			// Full path to executable
			var sb = new StringBuilder(1024);
			uint len = (uint)sb.Capacity + 1;
			Win32Interop.QueryFullProcessImageName(limitedHandle, 0, sb, ref len);
			this.FullProcessName = sb.ToString();

			// Close handle
			Win32Interop.CloseHandle(limitedHandle);
		}

		private void UpdateVariableData()
		{
			// Title bar name
			this.Name = Win32Interop.GetWindowText(Handle);

			// Window dimensions
			Win32Interop.GetWindowRect(Handle, out Rect rect);
			this.Bottom = rect.Bottom;
			this.Left = rect.Left;
			this.Right = rect.Right;
			this.Top = rect.Top;

			// Extended window styles
			this.ExStyle = Win32Interop.GetWindowLongPtr(Handle, (int)GWL.GWL_EXSTYLE);
		}
	}
}

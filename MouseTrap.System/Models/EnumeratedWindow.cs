using MouseTrap.Interop;
using System;

namespace MouseTrap.Models
{
	internal class EnumeratedWindow : IWindow
	{
		private uint _processId;
		private string _processPath;
		private string _title;
		private double? _left;
		private double? _top;
		private double? _right;
		private double? _bottom;
		private bool? _isMinimized;

		public EnumeratedWindow(IntPtr hWnd)
		{
			Handle = hWnd;
			NativeMethods.GetWindowThreadProcessId(Handle, out _processId);
		}

		public IntPtr Handle { get; private set; }

		public uint ProcessId => _processId;

		public string ProcessPath
		{
			get
			{
				if (_processPath == null) _processPath = NativeMethods.GetFullProcessName((int)ProcessId);
				return _processPath;
			}
		}

		public string Title
		{
			get
			{
				if (_title == null) _title = NativeMethods.GetWindowText(Handle);
				return _title;
			}
		}

		public double Left
		{
			get
			{
				if (!_left.HasValue) GetWindowRect();
				return _left.GetValueOrDefault();
			}
		}

		public double Top
		{
			get
			{
				if (!_top.HasValue) GetWindowRect();
				return _top.GetValueOrDefault();
			}
		}

		public double Right
		{
			get
			{
				if (!_right.HasValue) GetWindowRect();
				return _right.GetValueOrDefault();
			}
		}

		public double Bottom
		{
			get
			{
				if (!_bottom.HasValue) GetWindowRect();
				return _bottom.GetValueOrDefault();
			}
		}

		public bool IsMinimized
		{
			get
			{
				if (!_isMinimized.HasValue) _isMinimized = NativeMethods.IsIconic(Handle);
				return _isMinimized.GetValueOrDefault();
			}
		}

		private void GetWindowRect()
		{
			NativeMethods.GetWindowRect(Handle, out Win32Rect rect);
			_left = rect.Left;
			_top = rect.Top;
			_right = rect.Right;
			_bottom = rect.Bottom;
		}
	}
}

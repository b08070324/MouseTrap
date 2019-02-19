using MouseTrap.Interop;
using MouseTrap.Models;
using System;

namespace MouseTrap.Models
{
	internal class ApplicationState : IApplicationState
	{
		private uint _processId;

		public IntPtr Handle { get; private set; }
		public uint ProcessId { get => _processId; private set => _processId = value; }
		public string ProcessPath { get; private set; }
		public Dimensions Padding { get; private set; }

		public ApplicationState()
		{
			Padding = new Dimensions();
		}

		public bool IsWatchingProgramPath
		{
			get => (Handle == default && ProcessId == default && ProcessPath != default);
		}

		public bool IsWatchingSpecificWindow
		{
			get => (Handle != default && ProcessId != default && ProcessPath != default);
		}

		public void WatchForProgramPath(string processPath)
		{
			if (ProcessPath == default)
			{
				Handle = default;
				ProcessId = default;
				ProcessPath = processPath;
				WatchingProgramPath?.Invoke(this, EventArgs.Empty);
			}
		}

		public void WatchForSpecificWindow(IntPtr handle)
		{
			if (Handle == default)
			{
				Handle = handle;
				NativeMethods.GetWindowThreadProcessId(handle, out _processId);
				ProcessPath = NativeMethods.GetFullProcessName((int)_processId);
				WatchingSpecificWindow?.Invoke(this, EventArgs.Empty);
			}
		}

		public void CancelWatch(bool windowWasClosed = false)
		{
			if (ProcessPath != default || Handle != default)
			{
				Handle = default;
				ProcessId = default;
				ProcessPath = default;
				WatchingCancelled?.Invoke(this, new WatchingCancelledEventArgs { WindowWasClosed = windowWasClosed });
			}
		}

		public void SetPadding(Dimensions padding)
		{
			Padding = padding;
			PaddingUpdated?.Invoke(this, EventArgs.Empty);
		}

		public void SetPadding(double left, double top, double right, double bottom)
		{
			SetPadding(new Dimensions { Left = left, Top = top, Right = right, Bottom = bottom });
		}


		public event EventHandler WatchingProgramPath;
		public event EventHandler WatchingSpecificWindow;
		public event EventHandler<WatchingCancelledEventArgs> WatchingCancelled;
		public event EventHandler PaddingUpdated;
	}
}

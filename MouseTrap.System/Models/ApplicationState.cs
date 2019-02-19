using MouseTrap.Interop;
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
			Handle = default;
			ProcessId = default;
			ProcessPath = processPath;
			WatchingProgramPath?.Invoke(this, EventArgs.Empty);
		}

		public void WatchForSpecificWindow(IntPtr handle, bool startRestricted)
		{
			Handle = handle;
			NativeMethods.GetWindowThreadProcessId(handle, out _processId);
			ProcessPath = NativeMethods.GetFullProcessName((int)_processId);
			WatchingSpecificWindow?.Invoke(this, EventArgs.Empty);
		}

		public void CancelWatch()
		{
			Handle = default;
			ProcessId = default;
			ProcessPath = default;
			WatchingCancelled?.Invoke(this, EventArgs.Empty);
		}

		public void SetPadding(Dimensions padding)
		{
			Padding = padding;
			PaddingUpdated?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler WatchingProgramPath;
		public event EventHandler WatchingSpecificWindow;
		public event EventHandler WatchingCancelled;
		public event EventHandler PaddingUpdated;
	}
}

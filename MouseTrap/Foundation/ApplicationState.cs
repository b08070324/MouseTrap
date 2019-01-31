using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MouseTrap.Models;

namespace MouseTrap.Foundation
{
	public class ApplicationState
	{
		private IMediator _mediator;
		private ViewType _previousView;

		public ApplicationState(IMediator mediator)
		{
			_mediator = mediator;
		}

		public ObservableCollection<IWindowItem> WindowList { get; private set; }
		public ViewType CurrentView { get; private set; }
		public IWindowItem TargetWindow { get; private set; }
		public IWindowItem ForegroundWindow { get; private set; }
		public Dimensions BoundaryOffset { get; private set; } = new Dimensions(8, 8, 8, 8);

		public bool IsLockEnabled
		{
			get => CurrentView == ViewType.LockWindow;
		}

		public bool IsTargetWindowFocused
		{
			get
			{
				if (TargetWindow == null) return false;
				return TargetWindow.IsMatch(ForegroundWindow);
			}
		}

		public bool SetCurrentView(ViewType viewType)
		{
			// No change
			if (CurrentView == viewType) return false;

			// Switching to lock window has requirements
			if (viewType == ViewType.LockWindow)
			{
				// Target is full, from WindowList, and still exists
				// Target is partial, from FindProgram
				if (TargetWindow.IsPathValid || (TargetWindow.IsValid && _mediator.IsWindowValid(TargetWindow)))
				{
					// Get latest details before hook updates
					_mediator.RefreshWindowDetails();
				}
				else
				{
					// Window no longer exists, cannot lock
					_mediator.RefreshWindowList();
					return false;
				}
			}
			else if (viewType == ViewType.PreviousView)
			{
				CurrentView = _previousView;
				return true;
			}

			// Tidy up target
			if (TargetWindow != null)
			{
				if (viewType == ViewType.WindowList && !TargetWindow.IsValid)
				{
					SetTargetWindow(null);
				}
				else if (viewType == ViewType.FindProgram && !TargetWindow.IsPathValid)
				{
					SetTargetWindow(null);
				}
			}

			// Store last view and update
			_previousView = CurrentView;
			CurrentView = viewType;
			return true;
		}

		public bool SetTargetWindow(IWindowItem windowItem)
		{
			if (windowItem == null)
			{
				// Both are same, no change
				if (TargetWindow == null) return false;
			}
			else
			{
				// Both are same, no change
				if (windowItem.Equals(TargetWindow)) return false;
			}

			// Property changed
			TargetWindow = windowItem;
			return true;
		}

		public bool UpdateTargetWindow()
		{
			if (TargetWindow != null)
			{
				// Get update details via mediator
				var update = _mediator.GetWindowItemUpdate(TargetWindow);

				// Update target
				TargetWindow.Title = update.Title;
				TargetWindow.Dimensions = update.Dimensions;
				return true;
			}

			// No target to update
			return false;
		}

		public bool SetForegroundWindow(IWindowItem windowItem)
		{
			if (windowItem == null)
			{
				// Both are same, no change
				if (ForegroundWindow == null) return false;
			}
			else
			{
				// Both are same, no change
				if (windowItem.Equals(ForegroundWindow)) return false;
			}

			// Property changed
			ForegroundWindow = windowItem;
			return true;
		}

		public bool SetTargetWindowTitle(string title)
		{
			if (TargetWindow != null)
			{
				TargetWindow.Title = title;
				return true;
			}

			return false;
		}

		public bool SetTargetWindowRect(Dimensions value)
		{
			if (TargetWindow != null)
			{
				TargetWindow.Dimensions = value;
				return true;
			}

			return false;
		}

		public void RefreshWindowList()
		{
			// Store handle to selected window
			IntPtr handle = TargetWindow == null ? IntPtr.Zero : TargetWindow.Handle;

			// Update list with new objects
			WindowList = _mediator.GetWindowList();

			// Reset selected window
			if (handle != IntPtr.Zero)
			{
				var item = WindowList.FirstOrDefault(x => x.Handle == handle);
				if (item != null) TargetWindow = item;
			}
		}

		public bool ChangeViewAfterTargetWindowLost()
		{
			// Clear target
			TargetWindow = null;

			// Refresh list to remove missing target
			_mediator.RefreshWindowList();

			// Return view to previous
			if (CurrentView == ViewType.LockWindow)
			{
				return SetCurrentView(_previousView);
			}

			// Otherwise remain on view
			return false;
		}

		public bool SetBoundaryOffset(Dimensions dim)
		{
			if (BoundaryOffset.Equals(dim)) return false;
			BoundaryOffset = dim;
			return true;
		}
	}
}

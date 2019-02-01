using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MouseTrap.Models;

namespace MouseTrap.Foundation
{
	public class ApplicationState : INotifyPropertyChanged
	{
		// Fields
		private IMediator _mediator;
		private ViewType _previousView;

		// Initialisation
		public ApplicationState(IMediator mediator)
		{
			_mediator = mediator;
		}

		// Events
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		// Queries
		public ObservableCollection<IWindowItem> WindowList { get; private set; }
		public ViewType CurrentView { get; private set; }
		public IWindowItem TargetWindow { get; private set; }
		public IWindowItem ForegroundWindow { get; private set; }
		public Dimensions BoundaryOffset { get; private set; } = new Dimensions(8, 8, 8, 8);

		public bool IsLockEnabled => (CurrentView == ViewType.LockWindow);

		public bool IsTargetWindowFocused
		{
			get
			{
				if (TargetWindow == null) return false;
				return TargetWindow.IsMatch(ForegroundWindow);
			}
		}

		// Commands
		public void SetCurrentView(ViewType viewType)
		{
			// No change
			if (CurrentView == viewType) return;

			// Use the previous view
			if (viewType == ViewType.PreviousView)
			{
				viewType = _previousView;
			}

			// Switching to lock window has requirements
			if (viewType == ViewType.LockWindow)
			{
				// Target is full, from WindowList, and still exists
				// Target is partial, from FindProgram
				if (TargetWindow.IsPathValid || (TargetWindow.IsValid && _mediator.DoesWindowExist(TargetWindow)))
				{
					// Get latest details before hook updates
					_mediator.RefreshWindowDetails();
				}
				else
				{
					// Window no longer exists, cannot lock
					_mediator.RefreshWindowList();
					return;
				}
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
			OnPropertyChanged(nameof(CurrentView));
		}

		public void SetTargetWindow(IWindowItem windowItem)
		{
			if (windowItem == null)
			{
				// Both are same, no change
				if (TargetWindow == null) return;
			}
			else
			{
				// Both are same, no change
				if (windowItem.Equals(TargetWindow)) return;
			}

			// Property changed
			TargetWindow = windowItem;
			OnPropertyChanged(nameof(TargetWindow));
		}

		public void UpdateTargetWindow()
		{
			// No target to update
			if (TargetWindow == null) return;

			// Get update details via mediator
			var update = _mediator.GetWindowItemUpdate(TargetWindow);

			// Update target
			TargetWindow.Title = update.Title;
			TargetWindow.Dimensions = update.Dimensions;
			OnPropertyChanged(nameof(TargetWindow));
		}

		public void SetForegroundWindow(IWindowItem windowItem)
		{
			if (windowItem == null)
			{
				// Both are same, no change
				if (ForegroundWindow == null) return;
			}
			else
			{
				// Both are same, no change
				if (windowItem.Equals(ForegroundWindow)) return;
			}

			// Property changed
			ForegroundWindow = windowItem;
			OnPropertyChanged(nameof(ForegroundWindow));

			// Update window details if needed
			if (!TargetWindow.IsValid && TargetWindow.IsPathValid)
			{
				if (TargetWindow.MatchByProcessPath(ForegroundWindow))
				{
					_mediator.RefreshWindowDetails();
				}
			}
		}

		public void SetTargetWindowTitle(string title)
		{
			if (TargetWindow == null) return;

			TargetWindow.Title = title;
			OnPropertyChanged(nameof(TargetWindow));
		}

		public void SetTargetWindowRect(Dimensions value)
		{
			if (TargetWindow == null) return;

			TargetWindow.Dimensions = value;
			OnPropertyChanged(nameof(TargetWindow));
		}

		public void RefreshWindowList()
		{
			// Store handle to selected window
			IntPtr handle = TargetWindow == null ? IntPtr.Zero : TargetWindow.Handle;

			// Update list with new objects
			WindowList = _mediator.GetWindowListUpdate();
			OnPropertyChanged(nameof(WindowList));

			// Reset selected window
			if (handle != IntPtr.Zero)
			{
				var item = WindowList.FirstOrDefault(x => x.Handle == handle);
				if (item != null)
				{
					TargetWindow = item;
					OnPropertyChanged(nameof(TargetWindow));
				}
			}
		}

		public void ChangeViewAfterTargetWindowLost()
		{
			// Clear target
			TargetWindow = null;
			OnPropertyChanged(nameof(TargetWindow));

			// Refresh list to remove missing target
			_mediator.RefreshWindowList();

			// Return view to previous
			if (CurrentView == ViewType.LockWindow)
			{
				SetCurrentView(_previousView);
			}
		}

		public void SetBoundaryOffset(Dimensions dim)
		{
			if (BoundaryOffset.Equals(dim)) return;
			BoundaryOffset = dim;
			OnPropertyChanged(nameof(BoundaryOffset));
		}
	}
}

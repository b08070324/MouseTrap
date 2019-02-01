using System;
using MouseTrap.Foundation;
using MouseTrap.Interop;
using MouseTrap.Models;

namespace MouseTrap.Hooks
{
	public class HookManager
	{
		private IMediator _mediator;
		private ForegroundHook _foregroundHook;
		private UpdateHook _updateHook;
		private MouseHook _mouseHook;

		public HookManager(IMediator mediator)
		{
			_mediator = mediator;
			_mediator.PropertyChanged += Mediator_PropertyChanged;

			_foregroundHook = new ForegroundHook();
			_foregroundHook.ForegroundWindowChanged += ForegroundHook_ForegroundWindowChanged;

			_updateHook = new UpdateHook();
			_updateHook.WindowTitleChanged += UpdateHook_WindowTitleChanged;
			_updateHook.WindowRectChanged += UpdateHook_WindowRectChanged;
			_updateHook.WindowClosed += UpdateHook_WindowClosed;

			_mouseHook = new MouseHook();
			UpdateMouseHookMargin();
		}

		private void Mediator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(IMediator.CurrentView):
					OnViewChanged();
					break;
				case nameof(IMediator.TargetWindow):
					UpdateMouseHookRegion();
					break;
				case nameof(IMediator.ForegroundWindow):
					OnForegroundWindowUpdated();
					break;
				case nameof(IMediator.BoundaryOffset):
					UpdateMouseHookMargin();
					break;
				case nameof(IMediator.WindowList):
					break;
				case nameof(IMediator.AppClosing):
					OnAppClosing();
					break;
			}
		}

		public void OnAppClosing()
		{
			_foregroundHook.StopHook();
			_updateHook.StopHook();
			_mouseHook.StopHook();
		}

		private void UpdateMouseHookRegion()
		{
			if (_mediator.TargetWindow == null) return;

			var region = _mediator.TargetWindow.Dimensions;

			_mouseHook.SetRegion(new Win32Rect
			{
				Left = (int)region.Left,
				Top = (int)region.Top,
				Right = (int)region.Right,
				Bottom = (int)region.Bottom
			});
		}

		private void UpdateMouseHookMargin()
		{
			var margin = _mediator.BoundaryOffset;

			_mouseHook.SetMargin(new Win32Rect
			{
				Left = (int)margin.Left,
				Top = (int)margin.Top,
				Right = (int)-margin.Right,
				Bottom = (int)-margin.Bottom
			});
		}

		public void OnForegroundWindowUpdated()
		{
			// If target window is not complete, wait for a matching FG window
			// Update target window and enable update hook
			if (!_mediator.TargetWindow.IsValid && _mediator.TargetWindow.IsPathValid)
			{
				if (_mediator.TargetWindow.MatchByProcessPath(_mediator.ForegroundWindow))
				{
					_updateHook.StartHook(_mediator.TargetWindow.Handle);
					_mediator.RefreshWindowDetails();
				}
			}

			// Check to start mouse hook
			if (_mediator.TargetWindow.IsValid)
			{
				if (_mediator.TargetWindow.IsMatch(_mediator.ForegroundWindow))
				{
					_mouseHook.TrapMouse = true;
				}
				else
				{
					_mouseHook.TrapMouse = false;
				}
			}
		}

		public void OnViewChanged()
		{
			if (_mediator.CurrentView == ViewType.LockWindow)
			{
				// Enable FG hook
				_foregroundHook.StartHook();

				// Enable window update hook
				if (_mediator.TargetWindow.IsValid)
				{
					_updateHook.StartHook(_mediator.TargetWindow.Handle);
				}

				// Start mouse hook
				_mouseHook.StartHook();
			}
			else
			{
				// Disable FG hook
				_foregroundHook.StopHook();

				// Disable window update hook
				_updateHook.StopHook();

				// Disable mouse hook
				_mouseHook.StopHook();
			}
		}

		private void ForegroundHook_ForegroundWindowChanged(object sender, ForegroundHookEventArgs e)
		{
			_mediator.SetForegroundWindow(new WindowItem
			{
				Handle = e.Handle,
				ProcessId = e.WindowThreadProcId,
				ProcessPath = e.ProcessPath,
				Title = Win32Interop.GetWindowText(e.Handle)
			});
		}

		private void UpdateHook_WindowTitleChanged(object sender, string title)
		{
			_mediator.SetTargetWindowTitle(title);
		}

		private void UpdateHook_WindowRectChanged(object sender, Win32Rect rect)
		{
			var dimensions = new Dimensions(rect.Left, rect.Top, rect.Right, rect.Bottom);
			_mediator.SetTargetWindowRect(dimensions);
		}

		private void UpdateHook_WindowClosed(object sender, EventArgs e)
		{
			_mediator.TargetWindowLost();
		}
	}
}

using System;
using System.Collections.ObjectModel;
using System.Windows;
using MouseTrap.Models;
using MouseTrap.WindowQuery;

namespace MouseTrap.Foundation
{
	public class ConcreteMediator : IMediator
	{
		// Fields
		protected ApplicationState _applicationState;
		protected IWindowQueryManager _windowQueryManager;

		// Initialisation
		public ConcreteMediator()
		{
		}

		public void SetApplicationState(ApplicationState applicationState)
		{
			_applicationState = applicationState;
		}

		public void SetWindowQueryManager(IWindowQueryManager queryManager)
		{
			_windowQueryManager = queryManager;
		}

		// Events
		public event MediatorEventHandler OnWindowListUpdated;
		public event MediatorEventHandler OnForegroundWindowUpdated;
		public event MediatorEventHandler OnTargetWindowUpdated;
		public event MediatorEventHandler OnViewChanged;
		public event MediatorEventHandler OnBoundaryOffsetUpdated;
		public event MediatorEventHandler OnAppClosing;

		// Queries
		public ObservableCollection<IWindowItem> WindowList { get => _applicationState.WindowList; }
		public ViewType CurrentView { get => _applicationState.CurrentView; }
		public IWindowItem TargetWindow { get => _applicationState.TargetWindow; }
		public IWindowItem ForegroundWindow { get => _applicationState.ForegroundWindow; }
		public Dimensions BoundaryOffset { get => _applicationState.BoundaryOffset; }
		public bool IsLockEnabled { get => _applicationState.IsLockEnabled; }
		public bool IsTargetWindowFocused { get => _applicationState.IsTargetWindowFocused; }
		public bool IsWindowValid(IWindowItem windowItem) => _windowQueryManager.CheckWindow(windowItem);
		public ObservableCollection<IWindowItem> GetWindowList() => _windowQueryManager.GetWindowList();
		public WindowItemUpdateDetails GetWindowItemUpdate(IWindowItem windowItem) => _windowQueryManager.GetWindowItemUpdate(windowItem);

		// Commands
		public void RefreshWindowList()
		{
			_applicationState.RefreshWindowList();
			OnWindowListUpdated?.Invoke();
		}

		public void RefreshWindowDetails()
		{
			if (_applicationState.UpdateTargetWindow())
			{
				OnTargetWindowUpdated?.Invoke();
			}
		}

		public void SetBoundaryOffset(Dimensions value)
		{
			if (_applicationState.SetBoundaryOffset(value))
			{
				OnBoundaryOffsetUpdated?.Invoke();
			}
		}

		public void SetCurrentView(ViewType viewType)
		{
			if (_applicationState.SetCurrentView(viewType))
			{
				OnViewChanged?.Invoke();
			}
		}

		public void SetForegroundWindow(IWindowItem windowItem)
		{
			if (_applicationState.SetForegroundWindow(windowItem))
			{
				OnForegroundWindowUpdated?.Invoke();
			}
		}

		public void SetTargetWindow(IWindowItem windowItem)
		{
			if (_applicationState.SetTargetWindow(windowItem))
			{
				OnTargetWindowUpdated?.Invoke();
			}
		}

		public void SetTargetWindowTitle(string title)
		{
			if (_applicationState.SetTargetWindowTitle(title))
			{
				OnTargetWindowUpdated?.Invoke();
			}
		}

		public void SetTargetWindowRect(Dimensions value)
		{
			if (_applicationState.SetTargetWindowRect(value))
			{
				OnTargetWindowUpdated?.Invoke();
			}
		}

		public void TargetWindowLost()
		{
			if (_applicationState.ChangeViewAfterTargetWindowLost())
			{
				OnViewChanged?.Invoke();
			}
		}

		public void AppClosing()
		{
			OnAppClosing?.Invoke();
		}
	}
}

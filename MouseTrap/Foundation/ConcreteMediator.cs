using System.Collections.ObjectModel;
using System.ComponentModel;
using MouseTrap.Models;
using MouseTrap.WindowQuery;

namespace MouseTrap.Foundation
{
	public class ConcreteMediator : IMediator
	{
		// Fields
		protected ApplicationState _appState;
		protected IWindowQueryManager _windowManager;

		// Initialisation
		public ConcreteMediator()
		{
		}

		public void SetApplicationState(ApplicationState applicationState)
		{
			if (_appState != null) _appState.PropertyChanged -= ApplicationState_PropertyChanged;
			_appState = applicationState;
			_appState.PropertyChanged += ApplicationState_PropertyChanged;
		}

		public void SetWindowQueryManager(IWindowQueryManager queryManager)
		{
			_windowManager = queryManager;
		}

		// Queries
		public ObservableCollection<IWindowItem> WindowList => _appState.WindowList;
		public ViewType CurrentView => _appState.CurrentView;
		public IWindowItem TargetWindow => _appState.TargetWindow;
		public IWindowItem ForegroundWindow => _appState.ForegroundWindow;
		public Dimensions BoundaryOffset => _appState.BoundaryOffset;
		public bool IsLockEnabled => _appState.IsLockEnabled;
		public bool IsTargetWindowFocused => _appState.IsTargetWindowFocused;
		public bool DoesWindowExist(IWindowItem windowItem) => _windowManager.CheckWindow(windowItem);
		public ObservableCollection<IWindowItem> GetWindowListUpdate() => _windowManager.GetWindowList();
		public WindowItemUpdateDetails GetWindowItemUpdate(IWindowItem windowItem) => _windowManager.GetWindowItemUpdate(windowItem);

		// Commands
		public void RefreshWindowList() => _appState.RefreshWindowList();
		public void SetForegroundWindow(IWindowItem windowItem) => _appState.SetForegroundWindow(windowItem);
		public void RefreshWindowDetails() => _appState.UpdateTargetWindow();
		public void SetTargetWindow(IWindowItem windowItem) => _appState.SetTargetWindow(windowItem);
		public void SetTargetWindowTitle(string title) => _appState.SetTargetWindowTitle(title);
		public void SetTargetWindowRect(Dimensions value) => _appState.SetTargetWindowRect(value);
		public void SetCurrentView(ViewType viewType) => _appState.SetCurrentView(viewType);
		public void TargetWindowLost() => _appState.ChangeViewAfterTargetWindowLost();
		public void SetBoundaryOffset(Dimensions value) => _appState.SetBoundaryOffset(value);
		public void AppClosing() => OnPropertyChanged(nameof(AppClosing));

		// Events
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		// ApplicationState event handler
		private void ApplicationState_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(ApplicationState.WindowList):
					OnPropertyChanged(nameof(WindowList));
					break;
				case nameof(ApplicationState.CurrentView):
					OnPropertyChanged(nameof(CurrentView));
					break;
				case nameof(ApplicationState.TargetWindow):
					OnPropertyChanged(nameof(TargetWindow));
					break;
				case nameof(ApplicationState.ForegroundWindow):
					OnPropertyChanged(nameof(ForegroundWindow));
					break;
				case nameof(ApplicationState.BoundaryOffset):
					OnPropertyChanged(nameof(BoundaryOffset));
					break;
			}
		}
	}
}

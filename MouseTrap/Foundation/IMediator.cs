using MouseTrap.Models;
using MouseTrap.WindowQuery;
using System.Collections.ObjectModel;
using System.Windows;

namespace MouseTrap.Foundation
{
	public delegate void MediatorEventHandler();

	public interface IMediator
	{
		// Initialisation
		void SetApplicationState(ApplicationState applicationState);
		void SetWindowQueryManager(IWindowQueryManager queryManager);

		// Events
		event MediatorEventHandler OnWindowListUpdated;
		event MediatorEventHandler OnForegroundWindowUpdated;
		event MediatorEventHandler OnTargetWindowUpdated;
		event MediatorEventHandler OnViewChanged;
		event MediatorEventHandler OnBoundaryOffsetUpdated;
		event MediatorEventHandler OnAppClosing;

		// Queries
		ObservableCollection<IWindowItem> WindowList { get; }
		ViewType CurrentView { get; }
		IWindowItem TargetWindow { get; }
		IWindowItem ForegroundWindow { get; }
		Dimensions BoundaryOffset { get; }
		bool IsTargetWindowFocused { get; }
		bool IsLockEnabled { get; }
		bool IsWindowValid(IWindowItem windowItem);
		ObservableCollection<IWindowItem> GetWindowList();
		WindowItemUpdateDetails GetWindowItemUpdate(IWindowItem windowItem);

		// Commands
		void RefreshWindowList();
		void RefreshWindowDetails();
		void SetBoundaryOffset(Dimensions value);
		void SetCurrentView(ViewType viewType);
		void SetForegroundWindow(IWindowItem windowItem);
		void SetTargetWindow(IWindowItem windowItem);
		void SetTargetWindowTitle(string title);
		void SetTargetWindowRect(Dimensions value);
		void TargetWindowLost();
		void AppClosing();
	}
}

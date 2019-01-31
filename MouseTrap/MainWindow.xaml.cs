using System.ComponentModel;
using System.Windows;
using MouseTrap.Foundation;
using MouseTrap.Models;
using MouseTrap.ViewModels;

namespace MouseTrap
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		// View state properties
		public ToolBarViewModel ToolBarViewModel { get; set; }
		public BaseViewModel CurrentViewModel { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;

		// View models
		private WindowListViewModel windowListModel;
		private FindProgramViewModel findProgramModel;
		private LockWindowViewModel lockWindowModel;

		IMediator _mediator;

		// Constructor
		public MainWindow()
		{
			InitializeComponent();
			Loaded += MainWindow_Loaded;
			Closing += MainWindow_Closing;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			// Get mediator
			_mediator = ConcreteMediatorFactory.GetMediator();
			_mediator.OnViewChanged += OnViewChanged;

			// Create view models
			ToolBarViewModel = new ToolBarViewModel(_mediator);
			windowListModel = new WindowListViewModel(_mediator);
			findProgramModel = new FindProgramViewModel(_mediator);
			lockWindowModel = new LockWindowViewModel(_mediator);

			// Bind datacontext
			DataContext = this;

			// Show main view
			_mediator.RefreshWindowList();
			SetCurrentView(_mediator.CurrentView);
		}

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			_mediator.AppClosing();
		}

		// Change the current view based on the selected view model type
		public void SetCurrentView(ViewType currentView)
		{
			BaseViewModel newView = null;

			switch (currentView)
			{
				case ViewType.WindowList:
					newView = windowListModel;
					break;
				case ViewType.FindProgram:
					newView = findProgramModel;
					break;
				case ViewType.LockWindow:
					newView = lockWindowModel;
					break;
				default:
					break;
			}

			if (newView == null || newView == CurrentViewModel) return;
			CurrentViewModel = newView;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentViewModel"));
		}

		public void OnViewChanged()
		{
			SetCurrentView(_mediator.CurrentView);
		}
	}
}

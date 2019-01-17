using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using MouseTrap.Foundation;
using MouseTrap.Models;
using MouseTrap.Data;

namespace MouseTrap
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// Store ref to application
		private IMouseTrapApplication mouseTrapApplication;

		// General app variables for binding etc
		public WindowModel WindowModel { get; set; } = new WindowModel();
		private string searchText = string.Empty;

		// Datagrid binding
		private BatchedObservableCollection<WindowItem> observedWindowList = new BatchedObservableCollection<WindowItem>();
		public CollectionViewSource WindowListViewSource { get; set; } = new CollectionViewSource();

		// Threading
		private SynchronizationContext uiContext = SynchronizationContext.Current;
		private readonly BackgroundWorker worker;
		public bool isPolling;

		// Constructor
		public MainWindow()
		{
			InitializeComponent();

			mouseTrapApplication = Application.Current as IMouseTrapApplication;

			// View model binding
			WindowListViewSource.Source = observedWindowList;
			WindowListViewSource.Filter += CollectionViewSource_Filter;

			// Update thread
			worker = new BackgroundWorker();
			worker.WorkerSupportsCancellation = true;
			worker.DoWork += DoWork;
			worker.RunWorkerCompleted += RunWorkerCompleted;

			Loaded += MainWindow_Loaded;
			Closing += MainWindow_Closing;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			RefreshDataGrid();
		}

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			StopWorker();
		}

		private void RefreshDataGrid()
		{
			// Get list
			var list = mouseTrapApplication.GetWindowList();

			// Update UI etc
			uiContext.Send(x =>
			{
				WindowModel.MouseTrapRequested = false;
				WindowModel.SelectedIndex = -1;
				observedWindowList.SetItems(list);
			}
			, null);
		}

		private void StartWorker()
		{
			if (!isPolling)
			{
				isPolling = true;
				worker.RunWorkerAsync();
			}
		}

		private void StopWorker()
		{
			isPolling = false;
			worker.CancelAsync();
		}

		// This run in worker thread
		private void DoWork(object sender, DoWorkEventArgs e)
		{
			if (worker.CancellationPending)
			{
				e.Cancel = true;
			}
			else
			{
				// Update underlying data
				var updatedDetails = mouseTrapApplication.UpdateWindowDetails();
				if (updatedDetails != null)
				{
					// Update view model
					WindowModel.Title = updatedDetails.Title;
					WindowModel.Process = updatedDetails.Process;
					WindowModel.Top = updatedDetails.Top;
					WindowModel.Left = updatedDetails.Left;
					WindowModel.Width = updatedDetails.Width;
					WindowModel.Height = updatedDetails.Height;
					WindowModel.HasFocus = updatedDetails.HasFocus;

					// Update mouse hook
					mouseTrapApplication.SetMouseHookState(WindowModel.MouseTrapRequested);

					// Throttle polling
					Thread.Sleep(150);
				}
				else
				{
					StopWorker();
					RefreshDataGrid();
				}
			}
		}

		// This runs in UI thread
		private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (isPolling)
			{
				worker.RunWorkerAsync();
			}
		}

		private void RefreshWindowList_Click(object sender, RoutedEventArgs e)
		{
			RefreshDataGrid();
		}

		private void OutputGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			// Get selected window
			if (e.AddedCells.Count > 0)
			{
				var item = e.AddedCells.First().Item as WindowItem;
				mouseTrapApplication.SelectWindow(item);
			}
			else
			{
				mouseTrapApplication.SelectWindow(null);
			}

			// Start (or continue) polling if window was selected
			if (mouseTrapApplication.IsWindowSelected) StartWorker();
			else StopWorker();
		}

		private void ProcessNameInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			var box = e.Source as TextBox;
			searchText = box.Text;
			WindowListViewSource.View.Refresh();
		}

		private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
		{
			var item = e.Item as WindowItem;
			var titleMatches = item.Title.ToLower().Contains(searchText);
			var processMatches = item.ProcessName.ToLower().Contains(searchText);
			e.Accepted = titleMatches || processMatches;
		}

		private void TrapMouse_Click(object sender, RoutedEventArgs e)
		{
			WindowModel.MouseTrapRequested = !WindowModel.MouseTrapRequested;
		}
	}
}

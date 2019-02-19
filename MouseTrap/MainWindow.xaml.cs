using System.ComponentModel;
using System.Windows;
using MouseTrap.ViewModels;

namespace MouseTrap
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// Constructor
		public MainWindow(MainWindowViewModel viewModel) : base()
		{
			InitializeComponent();
			DataContext = viewModel;
		}
	}
}

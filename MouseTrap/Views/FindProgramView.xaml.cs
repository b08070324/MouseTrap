using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MouseTrap.Views
{
	/// <summary>
	/// Interaction logic for FindProgramView.xaml
	/// </summary>
	public partial class FindProgramView : UserControl
	{
		public FindProgramView()
		{
			InitializeComponent();
		}

		private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = true;
		}

		private void TextBox_PreviewDrop(object sender, DragEventArgs e)
		{
			var text = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (text != null)
			{
				var box = sender as TextBox;
				box.Text = text[0];
				box.Focus();
				box.SelectAll();

				// ICommand.CanExecute not always getting called
				// This forces the button bar to update
				CommandManager.InvalidateRequerySuggested();
			}
			e.Handled = true;
		}
	}
}

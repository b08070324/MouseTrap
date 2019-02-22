using MouseTrap.Foundation;
using MouseTrap.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace MouseTrap.ViewModels
{
	public abstract class WindowListViewModel : NotifyingObject, IViewModel
	{
		private IWindowListItem _selectedWindow;

		// Backing data source
		public ObservableCollection<IWindowListItem> WindowList { get; private set; }

		// Datagrid data source
		public ListCollectionView DataSource { get; private set; }

		// Datagrid entry selected by user
		public IWindowListItem SelectedWindow
		{
			get => _selectedWindow;
			set => SetAndRaiseEvent(ref _selectedWindow, value);
		}

		// Constructor
		public WindowListViewModel()
		{
			WindowList = new ObservableCollection<IWindowListItem>();
			DataSource = new ListCollectionView(WindowList);
			DataSource.GroupDescriptions.Add(new PropertyGroupDescription("IsMinimized", new MinimizedValueConverter()));
		}

		// Refreshes WindowList
		public abstract void RefreshList();

		// Converts IsMinimized bool to a text description
		private class MinimizedValueConverter : IValueConverter
		{
			public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			{
				var isMinimized = (bool)value;
				if (isMinimized) return "Minimized windows";
				return "Active windows";
			}

			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			{
				throw new NotImplementedException();
			}
		}
	}
}

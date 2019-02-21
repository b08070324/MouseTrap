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
		private static readonly MinimizedValueConverter minimizedValueConverter = new MinimizedValueConverter();

		public WindowListViewModel()
		{
			// Subscribe to change events in order to update DataSource
			WindowList = new ObservableCollection<IWindowListItem>();
			WindowList.CollectionChanged += WindowList_CollectionChanged;
		}

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

		// If WindowList was updated, refresh the datagrid source
		private void WindowList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			DataSource = new ListCollectionView(WindowList);
			DataSource.GroupDescriptions.Add(new PropertyGroupDescription("IsMinimized", minimizedValueConverter));
		}

		// Refreshes WindowList
		public virtual void RefreshList() { }

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

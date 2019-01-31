using MouseTrap.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MouseTrap.Binding
{
	public class ViewTypeToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var currentView = (ViewType)value;
			var targetViewType = (ViewType)parameter;
			bool visible = currentView == targetViewType;
			return (visible ? Visibility.Visible : Visibility.Collapsed);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

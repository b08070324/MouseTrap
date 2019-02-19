using MouseTrap.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MouseTrap.Binding
{
	public class DimensionsToThicknessConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var dimensions = (double[])value;

			double left = GetMargin(dimensions[0]);
			double right = GetMargin(dimensions[1]);
			double top = GetMargin(dimensions[2]);
			double bottom = GetMargin(dimensions[3]);

			return new Thickness(left, top, right, bottom);
		}

		private static double GetMargin(double value)
		{
			if (value > 0) return 16;		// inner margin
			else if (value < 0) return 2;	// outer margin
			else return 8;					// no margin
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

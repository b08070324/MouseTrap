using MouseTrap.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MouseTrap.Binding
{
	public class DimensionsToThicknessConverter : IValueConverter
	{
		private static readonly double noMargin = 8;
		private static readonly double innerMargin = 16;
		private static readonly double outerMargin = 2;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var dimensions = (Dimensions)value;

			var left = dimensions.Left > 0 ? innerMargin : noMargin;
			left = dimensions.Left < 0 ? outerMargin : left;

			var right = dimensions.Right > 0 ? innerMargin : noMargin;
			right = dimensions.Right < 0 ? outerMargin : right;

			var top = dimensions.Top > 0 ? innerMargin : noMargin;
			top = dimensions.Top < 0 ? outerMargin : top;

			var bottom = dimensions.Bottom > 0 ? innerMargin : noMargin;
			bottom = dimensions.Bottom < 0 ? outerMargin : bottom;

			return new Thickness(left, top, right, bottom);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
	public class WindowInfoModel : INotifyPropertyChanged
	{
		private string title = "Program title";
		private int top = 0;
		private int left = 0;
		private int width = 0;
		private int height = 0;

		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
				NotifyPropertyChanged();
			}
		}

		public int Top
		{
			get
			{
				return top;
			}
			set
			{
				top = value;
				NotifyPropertyChanged();
			}
		}

		public int Left
		{
			get
			{
				return left;
			}
			set
			{
				left = value;
				NotifyPropertyChanged();
			}
		}

		public int Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
				NotifyPropertyChanged();
			}
		}

		public int Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value;
				NotifyPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

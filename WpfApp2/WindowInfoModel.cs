using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp2
{
	public class WindowInfoModel : INotifyPropertyChanged
	{
		private string title = "Program title";
		private int top = 0;
		private int left = 0;
		private int width = 0;
		private int height = 0;

		public event PropertyChangedEventHandler PropertyChanged;

		private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
		{
			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public string Title
		{
			get { return title; }
			set { SetField(ref title, value); }
		}

		public int Top
		{
			get { return top; }
			set { SetField(ref top, value); }
		}

		public int Left
		{
			get { return left; }
			set { SetField(ref left, value); }
		}

		public int Width
		{
			get { return width; }
			set { SetField(ref width, value); }
		}

		public int Height
		{
			get { return height; }
			set { SetField(ref height, value); }
		}
	}
}

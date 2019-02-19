using System.ComponentModel;
using System.Windows.Input;

namespace MouseTrap.ViewModels
{
	public abstract class FindProgramViewModel : IViewModel, INotifyPropertyChanged
	{
		private string _filename = string.Empty;
		private bool _isFilenameValid = false;

		public string Filename
		{
			get => _filename;
			set
			{
				if (value != _filename)
				{
					_filename = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filename)));
				}
			}
		}

		public bool IsFilenameValid
		{
			get => _isFilenameValid;
			set
			{
				if (value != _isFilenameValid)
				{
					_isFilenameValid = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFilenameValid)));
				}
			}
		}

		public ICommand FindFileCommand { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

using MouseTrap.Foundation;
using System.ComponentModel;
using System.Windows.Input;

namespace MouseTrap.ViewModels
{
	public abstract class FindProgramViewModel : NotifyingObject, IViewModel
	{
		private string _filename = string.Empty;
		private bool _isFilenameValid = false;

		public string Filename
		{
			get => _filename;
			set => SetAndRaiseEvent(ref _filename, value);
		}

		public bool IsFilenameValid
		{
			get => _isFilenameValid;
			set => SetAndRaiseEvent(ref _isFilenameValid, value);
		}

		public ICommand FindFileCommand { get; set; }
	}
}

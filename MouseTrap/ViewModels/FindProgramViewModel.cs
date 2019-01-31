using System;
using System.Windows.Input;
using Microsoft.Win32;
using MouseTrap.Binding;
using MouseTrap.Foundation;
using MouseTrap.Models;

namespace MouseTrap.ViewModels
{
	public class FindProgramViewModel : BaseViewModel
	{
		private string _filename;
		private bool _isFilenameValid;

		public FindProgramViewModel(IMediator mediator) : base(mediator)
		{
			Filename = "";
			IsFilenameValid = false;
			FindFileCommand = new RelayCommand(x => ShowOpenFileDialog());
			PropertyChanged += FindProgramViewModel_PropertyChanged;
			mediator.OnViewChanged += Mediator_OnViewChanged;
		}

		private void Mediator_OnViewChanged()
		{
			if (_mediator.CurrentView == ViewType.FindProgram)
			{
				SetTargetFromFilename();
			}
		}

		private void FindProgramViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Filename))
			{
				SetTargetFromFilename();
			}
		}

		private void SetTargetFromFilename()
		{
			// Clean up ends of string
			var cleanFilename = _filename.Trim('\"').Trim();

			// If the filename has content, or the targetwindow has been set before
			if (!string.IsNullOrEmpty(cleanFilename) || _mediator.TargetWindow != null)
			{
				_mediator.SetTargetWindow(new WindowItem
				{
					Handle = IntPtr.Zero,
					ProcessId = 0,
					ProcessPath = cleanFilename
				});
			}

			// Set visual display of validity
			IsFilenameValid = (_mediator.TargetWindow?.IsPathValid).GetValueOrDefault(false);
		}

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

		public ICommand FindFileCommand { get; protected set; }

		protected void ShowOpenFileDialog()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*";
			if (dialog.ShowDialog() == true)
			{
				Filename = dialog.FileName;
			}
		}
	}
}

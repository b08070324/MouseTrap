using Microsoft.Win32;
using MouseTrap.Binding;
using System;
using System.IO;

namespace MouseTrap.ViewModels
{
	public class FindProgramLiveModel : FindProgramViewModel
	{
		public FindProgramLiveModel()
		{
			FindFileCommand = new RelayCommand(param => ShowOpenFileDialog());
			PropertyChanged += FindProgramLiveModel_PropertyChanged;
		}

		private void FindProgramLiveModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Filename))
			{
				IsFilenameValid = Filename != null ? CheckProcessPath(Filename) : false;
			}
		}

		private static bool CheckProcessPath(string filepath)
		{
			// Basic check for string
			if (string.IsNullOrEmpty(filepath)) return false;

			// Check filename has a valid directory
			try { if (string.IsNullOrEmpty(Path.GetDirectoryName(filepath))) return false; }
			catch (Exception) { return false; }

			// Check file exists
			return File.Exists(filepath);
		}

		private void ShowOpenFileDialog()
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
				Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*"
			};

			if (dialog.ShowDialog() == true)
			{
				Filename = dialog.FileName;
			}
		}
	}
}

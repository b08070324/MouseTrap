using MouseTrap.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace MouseTrap.ViewModels
{
	public abstract class ToolBarViewModel : IViewModel, INotifyPropertyChanged
	{
		private bool _windowLockEnabled;
		private ViewType _currentView;

		public bool WindowLockEnabled
		{
			get => _windowLockEnabled;
			set
			{
				if (value != _windowLockEnabled)
				{
					_windowLockEnabled = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowLockEnabled)));
				}
			}
		}

		public ViewType CurrentView
		{
			get => _currentView;
			set
			{
				if (value != _currentView)
				{
					_currentView = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentView)));
				}
			}
		}

		public ICommand ChooseWindowCommand { get; set; }
		public ICommand FindProgramCommand { get; set; }
		public ICommand ToggleLockCommand { get; set; }
		public ICommand RefreshListCommand { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler RefreshButtonClicked;

		protected void OnRefreshButtonClicked()
		{
			RefreshButtonClicked?.Invoke(this, EventArgs.Empty);
		}
	}
}

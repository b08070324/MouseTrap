using MouseTrap.Foundation;
using MouseTrap.Models;
using System;
using System.Windows.Input;

namespace MouseTrap.ViewModels
{
	public abstract class ToolBarViewModel : NotifyingObject, IViewModel
	{
		private bool _windowLockEnabled;
		private ViewType _currentView;

		public bool WindowLockEnabled
		{
			get => _windowLockEnabled;
			set => SetAndRaiseEvent(ref _windowLockEnabled, value);
		}

		public ViewType CurrentView
		{
			get => _currentView;
			set => SetAndRaiseEvent(ref _currentView, value);
		}

		public ICommand ChooseWindowCommand { get; set; }
		public ICommand FindProgramCommand { get; set; }
		public ICommand ToggleLockCommand { get; set; }
		public ICommand RefreshListCommand { get; set; }

		public event EventHandler RefreshButtonClicked;

		protected void OnRefreshButtonClicked()
		{
			RefreshButtonClicked?.Invoke(this, EventArgs.Empty);
		}
	}
}

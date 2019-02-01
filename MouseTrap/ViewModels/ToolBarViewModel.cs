using System.ComponentModel;
using System.Windows.Input;
using MouseTrap.Binding;
using MouseTrap.Foundation;
using MouseTrap.Models;

namespace MouseTrap.ViewModels
{
	public class ToolBarViewModel : BaseViewModel
	{
		public ToolBarViewModel(IMediator mediator) : base(mediator)
		{
			_mediator.PropertyChanged += Mediator_PropertyChanged;

			// Commands for view
			ChooseWindowCommand = new RelayCommand(x => ShowWindowList(), x => ShowWindowListCanExecute);
			FindProgramCommand = new RelayCommand(x => ShowFindProgram(), x => ShowFindProgramCanExecute);
			ToggleLockCommand = new RelayCommand(x => ToggleLock(), x => ToggleLockCanExecute);
			RefreshListCommand = new RelayCommand(x => RefreshList(), x => true);
		}

		private void Mediator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(IMediator.CurrentView))
			{
				Mediator_OnViewChanged();
			}
		}

		public void Mediator_OnViewChanged()
		{
			RaiseEvent(nameof(LockState));
			RaiseEvent(nameof(CurrentView));
		}

		// View state properties
		public virtual bool LockState
		{
			get => _mediator.CurrentView == ViewType.LockWindow;
		}

		public ViewType CurrentView
		{
			get => _mediator.CurrentView;
		}

		// Commands
		public ICommand ChooseWindowCommand { get; protected set; }
		public ICommand FindProgramCommand { get; protected set; }
		public ICommand ToggleLockCommand { get; protected set; }
		public ICommand RefreshListCommand { get; protected set; }

		protected void ShowWindowList()
		{
			_mediator.SetCurrentView(ViewType.WindowList);
		}

		protected void ShowFindProgram()
		{
			_mediator.SetCurrentView(ViewType.FindProgram);
		}

		protected void ToggleLock()
		{
			if (_mediator.CurrentView == ViewType.LockWindow)
			{
				_mediator.SetCurrentView(ViewType.PreviousView);
			}
			else
			{
				_mediator.SetCurrentView(ViewType.LockWindow);
			}
		}

		protected void RefreshList()
		{
			_mediator.RefreshWindowList();
		}

		protected bool ShowWindowListCanExecute
		{
			get => _mediator.CurrentView == ViewType.FindProgram;
		}

		protected bool ShowFindProgramCanExecute
		{
			get => _mediator.CurrentView == ViewType.WindowList;
		}

		protected bool ToggleLockCanExecute
		{
			get
			{
				// Always allow unlock
				if (_mediator.CurrentView == ViewType.LockWindow) return true;

				// Need full details to lock from WindowListView
				if (_mediator.CurrentView == ViewType.WindowList)
				{
					return _mediator.TargetWindow != null && _mediator.TargetWindow.IsValid;
				}

				// Only need path to match from FindProgramView
				if (_mediator.CurrentView == ViewType.FindProgram)
				{
					return _mediator.TargetWindow != null && _mediator.TargetWindow.IsPathValid;
				}

				// Default state
				return false;
			}
		}
	}
}

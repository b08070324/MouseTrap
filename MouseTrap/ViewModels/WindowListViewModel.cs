using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MouseTrap.Foundation;
using MouseTrap.Models;

namespace MouseTrap.ViewModels
{
	public class WindowListViewModel : BaseViewModel
	{
		public WindowListViewModel(IMediator mediator) : base(mediator)
		{
			_mediator.PropertyChanged += Mediator_PropertyChanged;
		}

		private void Mediator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(IMediator.TargetWindow):
					UpdateSelectedWindow();
					break;
				case nameof(IMediator.WindowList):
					UpdateSelectedWindow();
					break;
			}
		}

		private void UpdateSelectedWindow()
		{
			RaiseEvent(nameof(WindowList));
			RaiseEvent(nameof(SelectedWindow));
		}

		// Datagrid is bound to AppState.WindowList
		public ObservableCollection<IWindowItem> WindowList
		{
			get => _mediator.WindowList;
		}

		// Datagrid.SelectedWindow is bound to this
		public IWindowItem SelectedWindow
		{
			get => _mediator.TargetWindow;
			set => _mediator.SetTargetWindow(value);
		}
	}
}

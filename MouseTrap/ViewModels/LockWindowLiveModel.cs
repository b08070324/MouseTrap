using MouseTrap.Models;

namespace MouseTrap.ViewModels
{
	public class LockWindowLiveModel : LockWindowViewModel
	{
		private ITargetWindowDetails TargetWindowDetails { get; set; }

		public LockWindowLiveModel(ITargetWindowDetails targetWindowDetails)
		{
			TargetWindowDetails = targetWindowDetails;
			TargetWindowDetails.PropertyChanged += TargetWindowDetails_PropertyChanged;
			Title = TargetWindowDetails.WindowTitle;
			WindowHeight = TargetWindowDetails.WindowDimensions.Height;
			WindowWidth = TargetWindowDetails.WindowDimensions.Width;
		}

		~LockWindowLiveModel()
		{
			TargetWindowDetails.PropertyChanged -= TargetWindowDetails_PropertyChanged;
		}

		private void TargetWindowDetails_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(TargetWindowDetails.WindowTitle))
			{
				Title = TargetWindowDetails.WindowTitle;
			}
			else if (e.PropertyName == nameof(TargetWindowDetails.HasFocus))
			{
				WindowIsFocused = TargetWindowDetails.HasFocus;
			}
			else if (e.PropertyName == nameof(TargetWindowDetails.WindowDimensions))
			{
				WindowHeight = TargetWindowDetails.WindowDimensions.Height;
				WindowWidth = TargetWindowDetails.WindowDimensions.Width;
			}
		}
	}
}

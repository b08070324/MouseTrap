using MouseTrap.Models;
using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public class LockWindowLiveModel : LockWindowViewModel
	{
		private IApplicationState ApplicationState { get; }
		private ITargetWindowDetails TargetWindowDetails { get; }

		public LockWindowLiveModel(IApplicationState applicationState, ITargetWindowDetails targetWindowDetails)
		{
			// Store state ref
			ApplicationState = applicationState;

			// Set padding
			LeftOffset = ApplicationState.Padding.Left;
			TopOffset = ApplicationState.Padding.Top;
			RightOffset = ApplicationState.Padding.Right;
			BottomOffset = ApplicationState.Padding.Bottom;

			// Subscribe to TargetWindowDetails updates
			TargetWindowDetails = targetWindowDetails;
			TargetWindowDetails.PropertyChanged += TargetWindowDetails_PropertyChanged;

			// Set properties
			Title = TargetWindowDetails.WindowTitle;
			WindowHeight = TargetWindowDetails.WindowDimensions.Height;
			WindowWidth = TargetWindowDetails.WindowDimensions.Width;

			// Subscribe to input changes
			PropertyChanged += LockWindowLiveModel_PropertyChanged;
		}

		~LockWindowLiveModel()
		{
			TargetWindowDetails.PropertyChanged -= TargetWindowDetails_PropertyChanged;
		}

		// Set system padding from input changes
		private void LockWindowLiveModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(LeftOffset) || 
				e.PropertyName == nameof(TopOffset) || 
				e.PropertyName == nameof(RightOffset) || 
				e.PropertyName == nameof(BottomOffset))
			{
				ApplicationState.SetPadding(LeftOffset, TopOffset, RightOffset, BottomOffset);
			}
		}

		// Update display from system changes
		private void TargetWindowDetails_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(ITargetWindowDetails.WindowTitle))
			{
				Title = TargetWindowDetails.WindowTitle;
			}
			else if (e.PropertyName == nameof(ITargetWindowDetails.HasFocus))
			{
				WindowIsFocused = TargetWindowDetails.HasFocus;
			}
			else if (e.PropertyName == nameof(ITargetWindowDetails.WindowDimensions))
			{
				WindowHeight = TargetWindowDetails.WindowDimensions.Height;
				WindowWidth = TargetWindowDetails.WindowDimensions.Width;
			}
		}
	}
}

using System.Windows;
using System.Windows.Input;

namespace MouseTrap.Controls
{
	/// <summary>
	/// Interaction logic for BarButton.xaml
	/// </summary>
	public partial class BarButton : BarButtonBase
	{
		public BarButton()
		{
			InitializeComponent();
		}

		// Hiding design-time members with new
		public new string ButtonText
		{
			get { return (string)GetValue(ButtonTextProperty); }
			set { SetValue(ButtonTextProperty, value); }
		}

		public new string ButtonPadding
		{
			get { return (string)GetValue(ButtonPaddingProperty); }
			set { SetValue(ButtonPaddingProperty, value); }
		}

		public new ICommand PressCommand
		{
			get { return (ICommand)GetValue(PressCommandProperty); }
			set { SetValue(PressCommandProperty, value); }
		}

		public new bool IsHighlighted
		{
			get { return (bool)GetValue(IsHighlightedProperty); }
			set { SetValue(IsHighlightedProperty, value); }
		}

		public static readonly DependencyProperty ButtonTextProperty =
			DependencyProperty.Register("ButtonText", typeof(string), typeof(BarButton), new PropertyMetadata(null));

		public static readonly DependencyProperty ButtonPaddingProperty =
			DependencyProperty.Register("ButtonPadding", typeof(string), typeof(BarButton), new PropertyMetadata(null));

		public static readonly DependencyProperty PressCommandProperty =
			DependencyProperty.Register("PressCommand", typeof(ICommand), typeof(BarButton), new PropertyMetadata(null));

		public static readonly DependencyProperty IsHighlightedProperty =
			DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(BarButton), new PropertyMetadata(false));
	}
}

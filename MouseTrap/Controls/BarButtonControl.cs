using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MouseTrap.Controls
{
	public class BarButtonControl : Control
	{
		// Button image source
		public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
			"Image", typeof(ImageSource), typeof(BarButtonControl)
			);

		public ImageSource Image
		{
			get => (ImageSource)GetValue(ImageProperty);
			set => SetValue(ImageProperty, value);
		}

		// Image margin
		public static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register(
			"ImageMargin", typeof(Thickness), typeof(BarButtonControl)
			);

		public Thickness ImageMargin
		{
			get => (Thickness)GetValue(ImageMarginProperty);
			set => SetValue(ImageMarginProperty, value);
		}

		// Button text
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(BarButtonControl)
			);

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		// Hover background brush
		public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.Register(
			"HoverBackground", typeof(Brush), typeof(BarButtonControl),
			new PropertyMetadata(Brushes.Transparent)
			);

		public Brush HoverBackground
		{
			get => (Brush)GetValue(HoverBackgroundProperty);
			set => SetValue(HoverBackgroundProperty, value);
		}

		// Hover border brush
		public static readonly DependencyProperty HoverBorderProperty = DependencyProperty.Register(
			"HoverBorder", typeof(Brush), typeof(BarButtonControl),
			new PropertyMetadata(Brushes.Transparent)
			);

		public Brush HoverBorder
		{
			get => (Brush)GetValue(HoverBorderProperty);
			set => SetValue(HoverBorderProperty, value);
		}

		// Hover highlighted background brush
		public static readonly DependencyProperty HoverHighlightedBackgroundProperty = DependencyProperty.Register(
			"HoverHighlightedBackground", typeof(Brush), typeof(BarButtonControl),
			new PropertyMetadata(Brushes.Transparent)
			);

		public Brush HoverHighlightedBackground
		{
			get => (Brush)GetValue(HoverHighlightedBackgroundProperty);
			set => SetValue(HoverHighlightedBackgroundProperty, value);
		}

		// Pressed foreground brush
		public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.Register(
			"PressedForeground", typeof(Brush), typeof(BarButtonControl),
			new PropertyMetadata(Brushes.Black)
			);

		public Brush PressedForeground
		{
			get => (Brush)GetValue(PressedForegroundProperty);
			set => SetValue(PressedForegroundProperty, value);
		}

		// Pressed background brush
		public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(
			"PressedBackground", typeof(Brush), typeof(BarButtonControl),
			new PropertyMetadata(Brushes.Transparent)
			);

		public Brush PressedBackground
		{
			get => (Brush)GetValue(PressedBackgroundProperty);
			set => SetValue(PressedBackgroundProperty, value);
		}

		// Pressed border brush
		public static readonly DependencyProperty PressedBorderProperty = DependencyProperty.Register(
			"PressedBorder", typeof(Brush), typeof(BarButtonControl),
			new PropertyMetadata(Brushes.Transparent)
			);

		public Brush PressedBorder
		{
			get => (Brush)GetValue(PressedBorderProperty);
			set => SetValue(PressedBorderProperty, value);
		}

		// Disabled background brush
		public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
			"DisabledBackground", typeof(Brush), typeof(BarButtonControl),
			new PropertyMetadata(Brushes.Black)
			);

		public Brush DisabledBackground
		{
			get => (Brush)GetValue(DisabledBackgroundProperty);
			set => SetValue(DisabledBackgroundProperty, value);
		}

		// Disabled border brush
		public static readonly DependencyProperty DisabledBorderProperty = DependencyProperty.Register(
			"DisabledBorder", typeof(Brush), typeof(BarButtonControl),
			new PropertyMetadata(Brushes.Black)
			);

		public Brush DisabledBorder
		{
			get => (Brush)GetValue(DisabledBorderProperty);
			set => SetValue(DisabledBorderProperty, value);
		}

		// Disabled foreground brush
		public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(
			"DisabledForeground", typeof(Brush), typeof(BarButtonControl),
			new PropertyMetadata(Brushes.Black)
			);

		public Brush DisabledForeground
		{
			get => (Brush)GetValue(DisabledForegroundProperty);
			set => SetValue(DisabledForegroundProperty, value);
		}

		// Disabled image source
		public static readonly DependencyProperty DisabledImageProperty = DependencyProperty.Register(
			"DisabledImage", typeof(ImageSource), typeof(BarButtonControl)
			);

		public ImageSource DisabledImage
		{
			get => (ImageSource)GetValue(DisabledImageProperty);
			set => SetValue(DisabledImageProperty, value);
		}

		// IsPressed
		public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
			"IsPressed", typeof(bool), typeof(BarButtonControl)
			);

		public bool IsPressed
		{
			get => (bool)GetValue(IsPressedProperty);
			set => SetValue(IsPressedProperty, value);
		}

		// IsHighlighted
		public static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register(
			"IsHighlighted", typeof(bool), typeof(BarButtonControl)
			);

		public bool IsHighlighted
		{
			get => (bool)GetValue(IsHighlightedProperty);
			set => SetValue(IsHighlightedProperty, value);
		}

		// Command
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
			"Command", typeof(ICommand), typeof(BarButtonControl)
			);

		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set
			{
				SetValue(CommandProperty, value);
				IsEnabled = Command.CanExecute(this);
			}
		}

		// Static constructor
		static BarButtonControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(BarButtonControl), 
				new FrameworkPropertyMetadata(typeof(BarButtonControl))
			);
		}

		// Constructor
		public BarButtonControl() : base()
		{
			MouseDown += BarButtonControl_MouseDown;
			MouseUp += BarButtonControl_MouseUp;
			Loaded += BarButtonControl_Loaded;
		}

		private void BarButtonControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (Command != null)
			{
				Command.CanExecuteChanged += Command_CanExecuteChanged;
			}
		}

		private void Command_CanExecuteChanged(object sender, EventArgs e)
		{
			if (Command != null)
			{
				IsEnabled = Command.CanExecute(this);
			}
		}

		private void BarButtonControl_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// Don't capture mouse if the control is disabled
			if (!IsEnabled) return;

			// Capture the mouse to the control
			// This will allow us to tell if the user released the mouse outside the control
			UIElement el = (UIElement)sender;
			IsPressed = el.CaptureMouse();

			// Finished
			e.Handled = true;
		}

		private void BarButtonControl_MouseUp(object sender, MouseButtonEventArgs e)
		{
			// Get the capture state and release
			UIElement el = (UIElement)sender;
			el.ReleaseMouseCapture();

			// Execute the command if the mouse was pressed and is still within the control
			if (IsPressed && el.IsMouseOver)
			{
				if (Command != null && Command.CanExecute(this)) 
				{
					Command.Execute(this);
					CommandManager.InvalidateRequerySuggested();
				}
			}

			// Cancel press
			IsPressed = false;

			// Finished
			e.Handled = true;
		}
	}
}

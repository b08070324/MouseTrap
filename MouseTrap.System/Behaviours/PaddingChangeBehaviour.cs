﻿using MouseTrap.Hooks;
using MouseTrap.Interop;
using MouseTrap.Models;
using System;

using static System.Diagnostics.Debug;

namespace MouseTrap.Behaviours
{
	/// <summary>
	/// If specific window is picked, determine if UI changes padding
	/// If changed, update MouseHook with size and padding
	/// </summary>
	internal class PaddingChangeBehaviour : BaseBehaviour
	{
		private IApplicationState AppState { get; }
		private IMouseHook MouseHook { get; }

		public PaddingChangeBehaviour(IApplicationState appState,
			IMouseHook mouseHook)
		{
			AppState = appState;
			MouseHook = mouseHook;
			AppState.PaddingUpdated += AppState_PaddingUpdated;
		}

		private void AppState_PaddingUpdated(object sender, EventArgs e)
		{
			if (AppState.IsWatchingSpecificWindow)
			{
				// Trace
				WriteLine($"{nameof(PaddingChangeBehaviour)}.{nameof(AppState_PaddingUpdated)}");

				// Get window rect
				NativeMethods.GetWindowRect(AppState.Handle, out Win32Rect rect);

				// Convert rect to dimensions
				Dimensions region = new Dimensions
				{
					Left = rect.Left,
					Top = rect.Top,
					Right = rect.Right,
					Bottom = rect.Bottom
				};

				// Get padded dimensions
				region = region.GetPaddedDimensions(AppState.Padding);

				// Set region
				MouseHook.SetRegion(region);
			}
		}
	}
}

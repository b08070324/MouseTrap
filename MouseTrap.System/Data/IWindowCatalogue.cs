﻿using MouseTrap.Models;
using System;

namespace MouseTrap.Data
{
	/// <summary>
	/// Interface to obtain a list of visible windows
	/// </summary>
	public interface IWindowCatalogue
	{
		/// <summary>
		/// Enumerates visible windows
		/// </summary>
		/// <param name="callback">Action to be called when a valid window is found</param>
		void EnumerateWindows(Action<IWindow> callback);
	}
}

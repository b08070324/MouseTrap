using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MouseTrap.Models
{
	public class WindowItem : IWindowItem
	{
		private IntPtr _handle;
		private uint _processId;
		private string _processPath;
		private string _title;
		private Dimensions _dimensions;

		// Properties
		public IntPtr Handle { get => _handle; set => _handle = value; }
		public uint ProcessId { get => _processId; set => _processId = value; }
		public string ProcessPath { get => _processPath; set => _processPath = value; }
		public string Title { get => _title; set => _title = value; }
		public Dimensions Dimensions { get => _dimensions; set => _dimensions = value; }

		// Derived properties
		public double Width => (_dimensions.Right - _dimensions.Left);
		public double Height => (_dimensions.Bottom - _dimensions.Top);
		public string ShortPath => System.IO.Path.GetFileName(_processPath);
		public BitmapSource ProcessIcon => WindowIcon.GetIcon(_processPath);
		public bool IsValid => (_handle != IntPtr.Zero && _processId != 0 && CheckProcessPath(_processPath));
		public bool IsPathValid => (_handle == IntPtr.Zero && _processId == 0 && CheckProcessPath(_processPath));

		public bool IsMatch(IWindowItem item)
		{
			if (item != null && item.IsValid && IsValid)
			{
				return item.Handle == _handle && item.ProcessId == _processId && ComparePaths(item.ProcessPath);
			}

			return false;
		}

		// If the path matches, this item is updated with details from given item
		public bool MatchByProcessPath(IWindowItem item)
		{
			if (IsPathValid && item.IsValid && ComparePaths(item.ProcessPath))
			{
				_handle = item.Handle;
				_processId = item.ProcessId;
				_title = item.Title;
				return true;
			}

			return false;
		}

		private bool ComparePaths(string testProcessPath)
		{
			return 0 == string.Compare(_processPath, testProcessPath, true);
		}

		public bool Equals(IWindowItem windowItem)
		{
			if (windowItem == null) return false;
			return (_handle == windowItem.Handle && _processId == windowItem.ProcessId &&
				_processPath == windowItem.ProcessPath && _title == windowItem.Title &&
				_dimensions.Equals(windowItem.Dimensions));
		}

		public IWindowItem Clone()
		{
			return (WindowItem)this.MemberwiseClone();
		}

		private static bool CheckProcessPath(string filepath)
		{
			// Basic check for string
			if (string.IsNullOrEmpty(filepath))
			{
				return false;
			}

			// Check filename has a valid directory
			try
			{
				if (string.IsNullOrEmpty(System.IO.Path.GetDirectoryName(filepath)))
				{
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}

			// Check file exists
			if (System.IO.File.Exists(filepath))
			{
				return true;
			}

			return false;
		}
	}
}

using MouseTrap.Interop;

namespace MouseTrap.Foundation
{
	// Extended data for a specific window
	public class DetailedWindowItem : WindowItem
	{
		public string FullProcessName { get; set; }
		public Rect BoundingDimensions { get; set; }
		public bool IsInForeground { get; set; }

		public DetailedWindowItem(WindowItem item)
		{
			Handle = item.Handle;
			ProcessId = item.ProcessId;
			ProcessName = item.ProcessName;
			Title = item.Title;
		}
	}
}

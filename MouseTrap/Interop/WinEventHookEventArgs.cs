using System;

namespace MouseTrap.Interop
{
	public class WinEventHookEventArgs : EventArgs
	{
		public WinEventConstant EventType { get; set; }
		public IntPtr Handle { get; set; }
		public int ObjectId { get; set; }
		public int ChildId { get; set; }

		public override string ToString()
		{
			return $"{EventType} hWnd {Handle} ObjId {ObjectId} ChildId {ChildId}";
		}
	}
}

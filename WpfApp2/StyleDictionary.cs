using System;
using System.Collections.Generic;

namespace WpfApp2
{
    public class StyleDictionary : Dictionary<string, bool>
	{
		public StyleDictionary(IntPtr exStyle)
		{
			Add("WS_EX_ACCEPTFILES", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_ACCEPTFILES));
			Add("WS_EX_APPWINDOW", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_APPWINDOW));
			Add("WS_EX_CLIENTEDGE", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_CLIENTEDGE));
			Add("WS_EX_COMPOSITED", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_COMPOSITED));
			Add("WS_EX_CONTEXTHELP", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_CONTEXTHELP));
			Add("WS_EX_CONTROLPARENT", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_CONTROLPARENT));
			Add("WS_EX_DLGMODALFRAME", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_DLGMODALFRAME));
			Add("WS_EX_LAYERED", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_LAYERED));
			Add("WS_EX_LAYOUTRTL", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_LAYOUTRTL));
			Add("WS_EX_LEFT", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_LEFT));
			Add("WS_EX_LEFTSCROLLBAR", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_LEFTSCROLLBAR));
			Add("WS_EX_LTRREADING", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_LTRREADING));
			Add("WS_EX_MDICHILD", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_MDICHILD));
			Add("WS_EX_NOACTIVATE", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_NOACTIVATE));
			Add("WS_EX_NOINHERITLAYOUT", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_NOINHERITLAYOUT));
			Add("WS_EX_NOPARENTNOTIFY", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_NOPARENTNOTIFY));
			Add("WS_EX_NOREDIRECTIONBITMAP", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_NOREDIRECTIONBITMAP));
			Add("WS_EX_OVERLAPPEDWINDOW", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_OVERLAPPEDWINDOW));
			Add("WS_EX_PALETTEWINDOW", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_PALETTEWINDOW));
			Add("WS_EX_RIGHT", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_RIGHT));
			Add("WS_EX_RIGHTSCROLLBAR", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_RIGHTSCROLLBAR));
			Add("WS_EX_RTLREADING", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_RTLREADING));
			Add("WS_EX_STATICEDGE", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_STATICEDGE));
			Add("WS_EX_TOOLWINDOW", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_TOOLWINDOW));
			Add("WS_EX_TOPMOST", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_TOPMOST));
			Add("WS_EX_TRANSPARENT", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_TRANSPARENT));
			Add("WS_EX_WINDOWEDGE", Win32Interop.HasExStyle(exStyle, Win32Interop.WindowStylesEx.WS_EX_WINDOWEDGE));
		}
	}
}

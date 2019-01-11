using System.Collections.Generic;

namespace WpfApp2
{
    public class WindowInformationComparer : IEqualityComparer<WindowInformation>
	{
		public bool Equals(WindowInformation a, WindowInformation b)
		{
			if (ReferenceEquals(a, b)) return true;
			if (a == null || b == null) return false;
			return a.Handle == b.Handle;
		}

		public int GetHashCode(WindowInformation windowInformation)
		{
			if (windowInformation == null) return 0;
			return windowInformation.Handle.GetHashCode();
		}
	}
}

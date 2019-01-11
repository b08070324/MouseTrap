using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class WindowInformation
	{
		public IntPtr Handle { get; set; }
		public string Name { get; set; }
		public uint ProcessId { get; set; }
		public string ProcessName { get; set; }
		public int Left { get; set; }
		public int Top { get; set; }
		public int Right { get; set; }
		public int Bottom { get; set; }
		public IntPtr ExStyle { get; set; }
	}
}

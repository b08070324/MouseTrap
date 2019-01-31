using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseTrap.Models
{
	public struct Dimensions
	{
		public double Left, Right, Top, Bottom;

		public Dimensions(double left, double top, double right, double bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + Left.GetHashCode();
			hash = (hash * 7) + Top.GetHashCode();
			hash = (hash * 7) + Right.GetHashCode();
			hash = (hash * 7) + Bottom.GetHashCode();
			return hash;
		}

		public override string ToString()
		{
			return $"{Left}, {Top}, {Right}, {Bottom}";
		}
	}
}

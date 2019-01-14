using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Models
{
	public class TrapMargin : ModelWithNotification
	{
		private int value = 0;

		public int Value
		{
			get { return value; }
			set { SetField(ref this.value, value); }
		}
	}
}

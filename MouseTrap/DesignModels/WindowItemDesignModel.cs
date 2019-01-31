using MouseTrap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MouseTrap.DesignModels
{
	public class WindowItemDesignModel : WindowItem
	{
		public WindowItemDesignModel()
		{
			ProcessPath = @"D:\Program Files(x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe";
			Dimensions = new Dimensions(0, 0, 1920, 1080);
		}
	}
}

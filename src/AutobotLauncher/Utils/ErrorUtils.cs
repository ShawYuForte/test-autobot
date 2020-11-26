using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutobotLauncher.Utils
{
	public static class ErrorUtils
	{
		public static void Error(this Exception ex)
		{
			Error(ex.Message, $"{ex.InnerException?.Message}, {ex.StackTrace}");
		}

		public static void Error(string name, string text)
		{
			var result = MessageBox.Show(text, name, MessageBoxButton.OK, MessageBoxImage.Error);
			//if (result == MessageBoxResult.Yes)
			//{
			//	Application.Current.Shutdown();
			//}
		}
	}
}

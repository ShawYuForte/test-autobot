using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace AutobotLauncher.Utils
{
	public static class FileUtils
	{
		public static DirectoryInfo Dir;

		static FileUtils()
		{
			Dir = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent;
		}

		public static string GetAbsolutePath(this string path)
		{
			return $"{Dir.FullName}\\{path}";
		}

		public static string GetClientPath(string v)
		{
			return $"{GetClientFolder(v)}\\device-cli.exe";
		}

		public static string GetClientFolder(string v)
		{
			return $"{Dir.FullName}\\device-cli.{v}\\tools\\cli";
		}
	}
}

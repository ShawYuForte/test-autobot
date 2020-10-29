using System;
using System.Diagnostics;

namespace AutobotLauncher.Utils
{
	public static class ProcessUtils
	{
		public static bool ProcessExists(this string name, string args = "")
		{
			var process = new Process
			{
				StartInfo =
				{
					FileName = name,
					UseShellExecute = false,
					CreateNoWindow = true,
					Arguments = args
				}
			};

			try
			{
				process.Start();
				return true;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return false;
		}

		public static void ProcessRunAndWaitAsAdmin(this string name, string args = "")
		{
			var process = new Process
			{
				StartInfo =
				{
					FileName = name,
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true,
					Arguments = args,
					Verb = "runas"
				}
			};

			try
			{
				process.OutputDataReceived += (s, e) =>
				{
					Console.WriteLine(e.Data);
				};

				process.Start();
				process.BeginOutputReadLine();
				process.WaitForExit();

				Console.WriteLine();

				if (process.ExitCode != 0)
				{
					Console.WriteLine($"Nuget process exited with code {process.ExitCode}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}

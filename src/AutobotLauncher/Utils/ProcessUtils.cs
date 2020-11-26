using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AutobotLauncher.Utils
{
	public static class ProcessUtils
	{
		public delegate void OnConsoleOutput(string value);

		public static async Task<List<string>> ProcessRunAndWaitAsAdmin(
			this string name, 
			string args = "", 
			bool wait = true, 
			OnConsoleOutput callback = null)
		{
			var r = new List<string>();

			var process = new Process
			{
				StartInfo =
				{
					WorkingDirectory = FileUtils.Dir.FullName,
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
					callback?.Invoke(e.Data);
					Console.WriteLine(e.Data);
					r.Add(e.Data);
				};

				process.Start();
				process.BeginOutputReadLine();

				if (wait)
				{
					await process.WaitForExitAsync();
				}

				return r;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				callback?.Invoke(null);
			}

			return null;
		}

		/// <summary>
		/// Waits asynchronously for the process to exit.
		/// </summary>
		/// <param name="process">The process to wait for cancellation.</param>
		/// <param name="cancellationToken">A cancellation token. If invoked, the task will return 
		/// immediately as canceled.</param>
		/// <returns>A Task representing waiting for the process to end.</returns>
		public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (process.HasExited) return Task.CompletedTask;

			var tcs = new TaskCompletionSource<object>();
			process.EnableRaisingEvents = true;
			process.Exited += (sender, args) => tcs.TrySetResult(null);
			if (cancellationToken != default(CancellationToken))
			{
				cancellationToken.Register(() => tcs.SetCanceled());
			}

			return process.HasExited ? Task.CompletedTask : tcs.Task;
		}
	}
}

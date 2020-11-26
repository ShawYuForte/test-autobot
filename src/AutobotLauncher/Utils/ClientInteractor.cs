using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutobotLauncher.Utils
{
	public static class ClientInteractor
	{
		private static string _errorMarker = "ERROR:";

		public static async Task<bool> StartClient(string v)
		{
			var started = false;
			var error = false;
			var args = $"run {GetDefaultParams(v)}";

			FileUtils.GetClientPath(v).ProcessRunAndWaitAsAdmin(args, true, (m) =>
			{
				if (m == null)
				{
					error = true;
				}
				else
				{
					started = true;
				}
			});

			while (!started && !error)
			{
				await Task.Delay(100);
			}

			if (started) { return true; }

			return false;
		}

		public static async Task<bool> CheckPreset(string v)
		{
			var args = $"run {GetDefaultParams(v)} --test-preset";

			var r = await FileUtils.GetClientPath(v).ProcessRunAndWaitAsAdmin(args);

			return !HasError(r);
		}

		public static async Task<bool> CheckApi(string v)
		{
			var args = $"run {GetDefaultParams(v)} --test-api";

			var r = await FileUtils.GetClientPath(v).ProcessRunAndWaitAsAdmin(args);

			return !HasError(r);
		}

		private static string GetDefaultParams(string v)
		{
			return $"-d \"{FileUtils.Dir.FullName}\\data\" -l \"{FileUtils.Dir.FullName}\\logs\" --pr \"{FileUtils.GetClientFolder(v)}\\preset\\Forte Preset.vmix\"";
		}

		private static bool HasError(List<string> appOutput)
		{
			return appOutput.Any(ao => ao != null && ao.Contains(_errorMarker));
		}
	}
}

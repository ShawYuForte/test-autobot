using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutobotLauncher.Utils
{
	public static class ClientApiInteractor
	{
		private static readonly string _clientAdress = "http://localhost:9000/api/device";

		public static async Task<string> Setting(string name)
		{
			return await Get($"{_clientAdress}/setting-text?name={name}");
		}

		public static async Task SettingSave(string name, string val, string type = "StringValue")
		{
			var r = await Post($"{_clientAdress}/settings-plain/{name}", new Dictionary<string, string>
			{
				[type] = val
			});
		}

		public static async Task Shutdown()
		{
			await Post($"{_clientAdress}/shutdown");
		}

		private static async Task<string> Post(string endpoint, Dictionary<string, string> values = null)
		{
			if (values == null)
			{
				values = new Dictionary<string, string>();
			}

			var client = new HttpClient();
			var content = new FormUrlEncodedContent(values);
			var response = await client.PostAsync(endpoint, content);

			if (!response.IsSuccessStatusCode)
			{
				ErrorUtils.Error(endpoint, await response.Content.ReadAsStringAsync());
				return null;
			}

			return await response.Content.ReadAsStringAsync();
		}

		private static async Task<string> Get(string endpoint, Dictionary<string, string> values = null)
		{
			var client = new HttpClient();
			var response = await client.GetAsync(endpoint);

			if (!response.IsSuccessStatusCode)
			{
				ErrorUtils.Error(endpoint, await response.Content.ReadAsStringAsync());
				return null;
			}

			return await response.Content.ReadAsStringAsync();
		}
	}
}

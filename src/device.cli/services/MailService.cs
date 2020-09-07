using System;
using System.Net.Mail;
using forte.devices.config;
using forte.services;

namespace forte.devices.services
{
	public class MailService
	{
		private ILogger _logger { get; set; }
		private IConfigurationManager _configManager { get; set; }

		public MailService(ILogger logger, IConfigurationManager configManager)
		{
			_logger = logger;
			_configManager = configManager;
		}

		public void MailError(string message, Exception ex)
		{
			var config = _configManager.GetDeviceConfig();
			var deviceName = config.Get<string>(SettingParams.DeviceName);

			var setts = System.Configuration.ConfigurationManager.AppSettings;

			var server = setts["mail:server"];
			var from = setts["mail:from"];
			var to = setts["mail:to"].Split(',');
			var user = setts["mail:user"];
			var password = setts["mail:password"];

			try
			{
				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient(server);

				mail.From = new MailAddress(from);
				mail.Subject = $"Autobot Error on {deviceName}";
				mail.Body = $"{deviceName} {Environment.NewLine} {message} {Environment.NewLine} Error details: {ex.Message} {ex.StackTrace}";
				foreach(var i in to)
				{
					var iTrimmed = i.Trim();
					#if DEBUG
					if(i == "ops@forte.fit") continue;
					#endif
					mail.To.Add(iTrimmed);
				}

				//SmtpServer.Port = 587;
				SmtpServer.Credentials = new System.Net.NetworkCredential(user, password);
				//SmtpServer.EnableSsl = true;

				SmtpServer.Send(mail);
			}
			catch(Exception exx)
			{
				_logger.Debug(exx, "");
			}
		}
	}
}

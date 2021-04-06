using System;
using System.Net.Mail;
using System.Threading.Tasks;
using forte.devices.config;
using forte.services;
using SendGrid;

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

        public async Task MailErrorAsync(string message, Exception ex)
        {
            var config = _configManager.GetDeviceConfig();
            var deviceName = config.Get<string>(SettingParams.DeviceName);

            var setts = System.Configuration.ConfigurationManager.AppSettings;
            var from = setts["mail:from"];
            var to = setts["mail:to"].Split(',');
            var apiKey = setts["mail:SendGrid:ApiKey"];

            try
            {
                var gridMessage = new SendGridMessage();
                foreach (var i in to)
                {
                    var iTrimmed = i.Trim();
                    #if DEBUG
                    if (i == "techsupport@forte.fit") continue;
                    #endif
                    gridMessage.AddTo(iTrimmed);
                }

                gridMessage.From = new MailAddress(from);
                gridMessage.Subject = $"Autobot Error on {deviceName}";
                gridMessage.Text = $"{deviceName} {Environment.NewLine} {message} {Environment.NewLine} Error details: {ex.Message} {ex.StackTrace}";

                var transportWeb = new Web(apiKey);
                await transportWeb.DeliverAsync(gridMessage);
            }
            catch (Exception exx)
            {
                _logger.Debug(exx, "");
            }
        }
    }
}
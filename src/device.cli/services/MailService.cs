using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using forte.devices.config;
using forte.services;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

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
            var from = new EmailAddress(setts["mail:from"], "From Autobot");
            var to = setts["mail:to"].Split(',');
            var recipients = new List<EmailAddress>();
            var apiKey = setts["mail:SendGrid:ApiKey"];

            try
            {

                foreach (var i in to)
                {
                    var iTrimmed = i.Trim();
                    #if DEBUG
                    if (i == "techsupport@forte.fit") continue;
                    #endif
                    recipients.Add(new EmailAddress(iTrimmed));
                }

                var subject = $"Autobot Error on {deviceName}";
                var client = new SendGridClient(apiKey);
                var plainTextContent = "";
                var htmlContent = $"<span style='color: red'>{deviceName} {Environment.NewLine} {message} {Environment.NewLine}</span>";
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, recipients, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                _logger.Debug(JsonConvert.SerializeObject(response));
                
            }
            catch (Exception exx)
            {
                _logger.Debug(exx, "");
            }
        }
    }
}
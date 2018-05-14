using Parsle.Models;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;
using Parsle.Interfaces;

namespace Parsle.SendInBlue
{
    /// <summary>
    /// Sends mail using SendInBlue
    /// </summary>
    public class MailService : IParsleService
    {
        private readonly string _sendInBlueApiKey;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sendInBlueApiKey">API used to send mail</param>
        public MailService(string sendInBlueApiKey)
        {
            sendInBlueApiKey.MustNotBeNullOrEmpty();
            _sendInBlueApiKey = sendInBlueApiKey;
        }

        /// <summary>
        /// Builds the dictionary entries which consitute the core properties of the mail
        /// </summary>
        /// <param name="mailMessage">The mail message to send</param>
        /// <returns>A set of dictionary entries that setup the main mail properties</returns>
        private Dictionary<string, object> BuildCoreMailProperties(ParsleMail mailMessage, bool isTemplate = false)
        {

            var fromName = new List<string>();
            var data = new Dictionary<string, object>();

            // create from address
            fromName.Add(mailMessage.From.Address);
            fromName.Add(mailMessage.From.DisplayName);

            // add to addresses
            var to = mailMessage.To.FirstOrDefault()?.Address;

            // add core email properties
            data.Add("to", to);
            data.Add("from", fromName);

            if (isTemplate) return data;

            data.Add("subject", mailMessage.Subject);
            data.Add("html", mailMessage.Html);

            return data;
        }

        private void DispatchMail(Dictionary<string, object> mailData)
        {
            var mailApi = new SendInBlueApi(_sendInBlueApiKey);

            if (mailData.ContainsKey("attr"))
            {
                var response = mailApi.send_transactional_template(mailData);
            }
            else
            {
                mailApi.send_email(mailData);
            }
        }

        public void SendMail(ParsleMail mailMessage)
        {
            // validate
            mailMessage.MustNotBeNull();
            mailMessage.Validate();

            // build basic mail properties
            var mailProps = BuildCoreMailProperties(mailMessage);

            // fire!
            DispatchMail(mailProps);
        }

        /// <summary>
        /// Sends a mail through SendInBlue using a template string replacements and a given template
        /// </summary>
        /// <param name="mailMessage">The message to send</param>
        /// <param name="templateId">The template to use</param>
        /// <param name="templateValues">The values to replace in the template</param>
        public void SendTemplateMail(ParsleMail mailMessage, string templateId, Dictionary<string, string> templateValues)
        {
            // validate
            mailMessage.MustNotBeNull();
            mailMessage.Validate();
            templateId.MustNotBeNullOrEmpty();
            templateValues.MustNotBeNull();

            // create core mail and header
            var mailProps = BuildCoreMailProperties(mailMessage, true);
            if (!string.IsNullOrEmpty(mailMessage.CustomHeaderValue)) mailProps.Add("X-Mailin-custom", mailMessage.CustomHeaderValue);

            // add template id and attributes to replace
            mailProps.Add("id", templateId);
            mailProps.Add("attr", templateValues);

            // fire!
            DispatchMail(mailProps);
        }

    }
}

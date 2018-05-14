using System.Collections.Generic;
using Parsle.Models;

namespace Parsle.Interfaces
{
    /// <summary>
    /// Defines an interface for a service that sends mail through a provider.
    /// </summary>
    public interface IParsleService
    {
        /// <summary>
        /// Sends the email using the service.
        /// </summary>
        /// <param name="mailMessage"></param>
        void SendMail(ParsleMail mailMessage);

        /// <summary>
        /// Sends a mail using a template on the provider system.
        /// </summary>
        /// <param name="mailMessage">The message to send</param>
        /// <param name="templateId">The template identifier</param>
        /// <param name="templateValues">Template values to set</param>
        void SendTemplateMail(ParsleMail mailMessage, string templateId, Dictionary<string, string> templateValues);
    }
}

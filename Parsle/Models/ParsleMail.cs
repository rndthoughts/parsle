using System.Net.Mail;
using Light.GuardClauses;

namespace Parsle.Models
{
    public class ParsleMail
    {
        public string CustomHeaderValue { get; set; }
        public MailAddress From { get; set; }
        public MailAddressCollection To { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public bool IsTemplate { get; set; }

        public ParsleMail(bool isTemplate)
        {
            To = new MailAddressCollection();
            IsTemplate = isTemplate;
        }

        public void Validate()
        {
            From.MustNotBeNull();
            To.MustHaveMinimumCount(1);

            if (IsTemplate) return;

            Subject.MustNotBeNullOrEmpty();
            Html.MustNotBeNullOrEmpty();
        }
    }
}

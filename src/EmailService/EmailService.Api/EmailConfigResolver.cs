using System.Collections;
using EmailService.Logic.Sending;

namespace EmailService.Api
{
    public class EmailConfigResolver : IEmailSenderConfig
    {
        public EmailConfigResolver(string defaultSenderEmail)
        {
            DefaultSenderEmail = defaultSenderEmail;
        }

        public string DefaultSenderEmail { get; }
    }
}
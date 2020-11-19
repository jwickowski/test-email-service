using System;
using System.Collections.Generic;
using System.Text;
using EmailService.Logic.UnitTests;

namespace EmailService.Logic
{
    public class EmailSaver
    {
        private readonly IEmailPersister _emailPersister;

        public EmailSaver(IEmailPersister emailPersister)
        {
            _emailPersister = emailPersister;
        }

        public Guid SaveEmail(EmailMessage message)
        {
            var processedMessage = ProcessMessage(message);
            return _emailPersister.PersistEmail(processedMessage, EmailSendingStatus.Pending);
        }

        private EmailMessage ProcessMessage(EmailMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.From))
            {
                return new EmailMessage(message.To, null, message.Topic, message.Content);
            }

            return message;
        }
    }
}

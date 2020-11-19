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
           return  _emailPersister.PersistEmail(message, EmailSendingStatus.Pending);
        }
    }
}

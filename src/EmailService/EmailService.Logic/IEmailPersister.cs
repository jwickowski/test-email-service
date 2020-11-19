using System;
using EmailService.Logic.UnitTests;

namespace EmailService.Logic
{
    public interface IEmailPersister
    {
        Guid PersistEmail(EmailMessage emailMessage, EmailSendingStatus emailSendingStatus);
    }
}
using System;

namespace EmailService.Logic
{
    public interface IEmailPersister
    {
        Guid PersistEmail(EmailMessage emailMessage, EmailSendingStatus emailSendingStatus);
    }
}
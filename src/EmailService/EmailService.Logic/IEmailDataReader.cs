using System;

namespace EmailService.Logic
{
    public interface IEmailDataReader
    {
        EmailSendingStatus GetEmailSendingStatus(Guid id);
        EmailMessage GetEmailMessage(Guid id);
    }
}
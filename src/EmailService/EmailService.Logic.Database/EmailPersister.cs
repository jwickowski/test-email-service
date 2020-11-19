using System;

namespace EmailService.Logic.Database
{
    public class EmailPersister : IEmailPersister
    {
        public  Guid PersistEmail(EmailMessage emailMessage, EmailSendingStatus emailSendingStatus)
        {
            throw new NotImplementedException();
        }
    }
}
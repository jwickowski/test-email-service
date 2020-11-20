using System;

namespace EmailService.Logic.Saving
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
            ValidateEmailMessage(message);
            var processedMessage = ProcessMessage(message);
            return _emailPersister.PersistEmail(processedMessage, EmailSendingStatus.Pending);
        }

        private void ValidateEmailMessage(EmailMessage message)
        {
            if (message.To == null || message.To.Length == 0)
            {
                throw new NoRecipientsException();
            }

            foreach (var to in message.To)
            {
                if (ValidateEmailAddress(to) == false)
                {
                    throw new ToFieldIsNotValidException();
                }
            }

            if (string.IsNullOrWhiteSpace(message.From) == false)
            {
                if (ValidateEmailAddress(message.From) == false)
                {
                    throw new FromFieldIsNotValidException();
                }
            }

        }

        private bool ValidateEmailAddress(string to)
        {
            if (to.IndexOf('@') == -1)
            {
                return false;
            }

            return true;
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

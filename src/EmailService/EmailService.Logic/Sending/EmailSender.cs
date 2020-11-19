using System;
using EmailService.Logic.UnitTests;

namespace EmailService.Logic.Sending
{
    public class EmailSender
    {
        private readonly IPendingEmailsGetter _pendingEmailsGetter;
        private readonly IProtocolEmailSender _protocolEmailSender;
        private readonly IEmailSenderConfig _emailSenderConfig;
        private readonly IEmailPersister _emailPersister;

        public EmailSender(IPendingEmailsGetter pendingEmailsGetter, IProtocolEmailSender protocolEmailSender,
            IEmailSenderConfig emailSenderConfig, IEmailPersister emailPersister)
        {
            _pendingEmailsGetter = pendingEmailsGetter;
            _protocolEmailSender = protocolEmailSender;
            _emailSenderConfig = emailSenderConfig;
            _emailPersister = emailPersister;
        }

        public void SendPendingEmails()
        {
            var pendingEmails = _pendingEmailsGetter.GetPendingMails();
            foreach (var emailToSend in pendingEmails)
            {
                var preparedEmail = ProcessEmail(emailToSend.EmailMessage);
                _protocolEmailSender.SendExternal(preparedEmail);
                _emailPersister.UpdateStatus(emailToSend.EmailId, EmailSendingStatus.Sent);
            }
        }

        private EmailMessage ProcessEmail(EmailMessage emailMessage)
        {
            if (string.IsNullOrWhiteSpace(emailMessage.From))
            {
                return new EmailMessage(emailMessage.To, _emailSenderConfig.DefaultSenderEmail, emailMessage.Topic, emailMessage.Content);
            }

            return emailMessage;
        }
    }
}
using System;

namespace EmailService.Logic.UnitTests
{
    internal class EmailSaverTestActResult
    {
        public Guid ReturnedMailId { get; set; }
        public EmailMessage PassedMessage { get; set; }
        public EmailSendingStatus PassedStatus { get; set; }
    }
}
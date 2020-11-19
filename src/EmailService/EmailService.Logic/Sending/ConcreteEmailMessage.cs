using System;

namespace EmailService.Logic.Sending
{
    public class ConcreteEmailMessage
    {
        public Guid EmailId { get; set; }
        public EmailMessage EmailMessage { get; set; }
    }
}

using System;

namespace EmailService.Logic.UnitTests
{
    public class ConcreteEmailMessage
    {
        public Guid EmailId { get; set; }
        public EmailMessage EmailMessage { get; set; }
    }
}
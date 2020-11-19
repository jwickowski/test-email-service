using System;
using NSubstitute;
using Xunit;

namespace EmailService.Logic.UnitTests
{
    public class EmailSenderTest
    {
        [Fact]
        public void when_email_is_sent_and_from_field_is_empty_then_use_default_from_config()
        {
            //var pendingEmailsGetter = Substitute.For<IPendingEmailsGetter>();
            //Guid emailId = _emailSender.SendPendingEmails();
            Assert.True(false);
        }
    }
}
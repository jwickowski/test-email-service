using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Transactions;
using NSubstitute;
using Xunit;

namespace EmailService.Logic.UnitTests
{
    public class EmailSenderTest
    {
        EmailMessage message = new EmailMessage(new[] { "to@wp.pl" }, "from@wp.pl", "Topic", "Hallo");


        [Fact]
        public void when_email_is_send_then_it_should_be_passed_to_protocol_sender()
        {
            var pendingEmailsGetter = Substitute.For<IPendingEmailsGetter>();
            var protocolEmailSender = Substitute.For<IProtocolEmailSender>();
            pendingEmailsGetter.GetPendingMails().Returns(new[] {new ConcreteEmailMessage{EmailId = Guid.NewGuid(), EmailMessage = message} });
            var _emailSender = new EmailSender(pendingEmailsGetter, protocolEmailSender);
            _emailSender.SendPendingEmails();

            protocolEmailSender.Received().SendExternal(Arg.Any<MailMessage>());
        }
        [Fact]
        public void when_email_is_sent_and_from_field_is_empty_then_use_default_from_config()
        {
    
            Assert.True(false);
        }
    }

    public class EmailSender
    {
        public EmailSender(IPendingEmailsGetter pendingEmailsGetter, IProtocolEmailSender protocolEmailSender)
        {
            throw new NotImplementedException();
        }

        public void SendPendingEmails()
        {
            throw new NotImplementedException();
        }
    }

    public interface IProtocolEmailSender
    {
        void SendExternal(MailMessage mail);
    }

    public interface IPendingEmailsGetter
    {
        public IEnumerable<ConcreteEmailMessage> GetPendingMails();
    }

    public class ConcreteEmailMessage
    {
        public Guid EmailId { get; set; }
        public EmailMessage EmailMessage { get; set; }
    }
}
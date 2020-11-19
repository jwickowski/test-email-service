using System;
using System.Collections;
using System.Linq;
using System.Net.Mail;
using System.Transactions;
using EmailService.Logic.Sending;
using NSubstitute;
using Xunit;

namespace EmailService.Logic.UnitTests
{
    public class EmailSenderTest
    {
        private IPendingEmailsGetter pendingEmailsGetter;
        private IProtocolEmailSender protocolEmailSender;
        private IEmailSenderConfig emailSenderConfig;
        private EmailSender _emailSender;

        EmailMessage message = new EmailMessage(new[] { "to@wp.pl" }, "from@wp.pl", "Topic", "Hallo");

        public EmailSenderTest()
        {
            pendingEmailsGetter = Substitute.For<IPendingEmailsGetter>();
            protocolEmailSender = Substitute.For<IProtocolEmailSender>();
            emailSenderConfig = Substitute.For<IEmailSenderConfig>();
            emailSenderConfig.DefaultSenderEmail.Returns("default@mail.com");
            _emailSender = new EmailSender(pendingEmailsGetter, protocolEmailSender);
        }

        [Fact]
        public void when_email_is_send_then_it_should_be_passed_to_protocol_sender()
        {
            var pendingMessages = new [] {new ConcreteEmailMessage {EmailId = Guid.NewGuid(), EmailMessage = message}};
            pendingEmailsGetter.GetPendingMails().Returns(pendingMessages);

            _emailSender.SendPendingEmails();

            protocolEmailSender.Received().SendExternal(Arg.Any<MailMessage>());
        }
        [Fact]
        public void when_email_is_sent_and_from_field_is_empty_then_use_default_from_config()
        {
            message = new EmailMessage(new[] { "to@wp.pl" }, null, "Topic", "Hallo");

            var pendingMessages = new[] { new ConcreteEmailMessage { EmailId = Guid.NewGuid(), EmailMessage = message } };
            pendingEmailsGetter.GetPendingMails().Returns(pendingMessages);

            _emailSender.SendPendingEmails();

            var sentEmail = protocolEmailSender.ReceivedCalls().First().GetArguments()[0] as EmailMessage;
            Assert.Equal("default@mail.com", sentEmail.From);
        }
    }

    public interface IEmailSenderConfig
    {
        public string DefaultSenderEmail { get; }
    }
}
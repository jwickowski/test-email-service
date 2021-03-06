﻿using System;
using System.Linq;
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
        private IEmailPersister _emailPersister;

        EmailMessage message = new EmailMessage(new[] { "to@wp.pl" }, "from@wp.pl", "Topic", "Hallo");

        public EmailSenderTest()
        {
            pendingEmailsGetter = Substitute.For<IPendingEmailsGetter>();
            protocolEmailSender = Substitute.For<IProtocolEmailSender>();
            emailSenderConfig = Substitute.For<IEmailSenderConfig>();
            _emailPersister = Substitute.For<IEmailPersister>();

            emailSenderConfig.DefaultSenderEmail.Returns("default@mail.com");

            _emailSender = new EmailSender(pendingEmailsGetter, protocolEmailSender, emailSenderConfig, _emailPersister);
        }

        private void Act()
        {
            _emailSender.SendPendingEmails();
        }

        [Fact]
        public void when_email_is_send_then_it_should_be_passed_to_protocol_sender()
        {
            var pendingMessages = new [] {new ConcreteEmailMessage {EmailId = Guid.NewGuid(), EmailMessage = message}};
            pendingEmailsGetter.GetPendingMails().Returns(pendingMessages);
            
            Act();

            protocolEmailSender.Received().SendExternal(Arg.Any<EmailMessage>());
        }
        [Fact]
        public void when_email_is_sent_and_from_field_is_empty_then_use_default_from_config()
        {
            message = new EmailMessage(new[] { "to@wp.pl" }, null, "Topic", "Hallo");

            var pendingMessages = new[] { new ConcreteEmailMessage { EmailId = Guid.NewGuid(), EmailMessage = message } };
            pendingEmailsGetter.GetPendingMails().Returns(pendingMessages);

            Act();

            var sentEmail = protocolEmailSender.ReceivedCalls().First().GetArguments()[0] as EmailMessage;
            Assert.Equal("default@mail.com", sentEmail.From);
        }

        [Fact]
        public void when_email_is_sent__then_ist_status_should_be_updated_as_send()
        {
            var emailId = Guid.NewGuid();
            var pendingMessages = new[] { new ConcreteEmailMessage { EmailId = emailId, EmailMessage = message } };
            pendingEmailsGetter.GetPendingMails().Returns(pendingMessages);

            Act();

            var sentEmail = protocolEmailSender.ReceivedCalls().First().GetArguments()[0] as EmailMessage;

            _emailPersister.Received().UpdateStatus(emailId, EmailSendingStatus.Sent);
        }

        [Fact]
        public void when_sending_emails_throw_an_exception_then_set_it_to_error()
        {
            var emailId = Guid.NewGuid();
            var pendingMessages = new[] { new ConcreteEmailMessage { EmailId = emailId, EmailMessage = message } };
            pendingEmailsGetter.GetPendingMails().Returns(pendingMessages);
            protocolEmailSender
                .When(x => x.SendExternal(Arg.Any<EmailMessage>()))
                .Do(x => throw new Exception());
            
            Act();

            _emailPersister.Received().UpdateStatus(emailId, EmailSendingStatus.Error);
        }
    }
}
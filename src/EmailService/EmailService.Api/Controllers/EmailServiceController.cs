using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using EmailService.Api.Models;
using EmailService.Logic;
using EmailService.Logic.Saving;
using EmailService.Logic.Sending;

namespace EmailService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailServiceController : ControllerBase
    {
        private readonly ILogger<EmailServiceController> _logger;
        private readonly EmailSaver _emailSaver;
        private readonly IEmailDataReader _emailDataReader;
        private readonly EmailSender _emailSender;

        public EmailServiceController(ILogger<EmailServiceController> logger, EmailSaver emailSaver, IEmailDataReader emailDataReader, EmailSender emailSender)
        {
            _logger = logger;
            _emailSaver = emailSaver;
            _emailDataReader = emailDataReader;
            _emailSender = emailSender;
        }

        [HttpGet("{id:Guid}")]
        public EmailMessageResponse GetEmailData(Guid id)
        {
            var status = _emailDataReader.GetEmailSendingStatus(id);
            var message = _emailDataReader.GetEmailMessage(id);
            var result = new EmailMessageResponse()
            {
                EmailId = id,
                EmailStatus = status.ToString(),
                From = message.From,
                Content = message.Content,
                Topic = message.Topic,
                To = message.To
            };

            return result;
        }

        [HttpPost("")]
        public Guid SaveEmail(EmailMessageRequestData emailMessage)
        {
            var message = new EmailMessage(emailMessage.To.ToArray(), emailMessage.From, emailMessage.Topic, emailMessage.Content);
            var newId = _emailSaver.SaveEmail(message);
            return newId;
        }

        [HttpPut("send-pending")]
        public void SendPending()
        {
            _emailSender.SendPendingEmails();
        }

        [HttpGet("all")]
        public IEnumerable<EmailMessageResponse> GetAll()
        {
            var allEmails = _emailDataReader.GetAll();
            var result = new List<EmailMessageResponse>();
            foreach (var anEmail in allEmails)
            {
                var item = new EmailMessageResponse()
                {
                    From = anEmail.EmailMessage.From,
                    Content = anEmail.EmailMessage.Content,
                    To = anEmail.EmailMessage.To,
                    Topic = anEmail.EmailMessage.Topic,
                    EmailId = anEmail.EmailId,
                    EmailStatus = _emailDataReader.GetEmailSendingStatus(anEmail.EmailId).ToString()
                };
                result.Add(item);
            }

            return result;
        }
    }
}

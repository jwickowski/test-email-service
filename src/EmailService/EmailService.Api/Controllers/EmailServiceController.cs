using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService.Logic;
using EmailService.Logic.Saving;

namespace EmailService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailServiceController : ControllerBase
    {
        private readonly ILogger<EmailServiceController> _logger;
        private readonly EmailSaver _emailSaver;

        public EmailServiceController(ILogger<EmailServiceController> logger, EmailSaver emailSaver)
        {
            _logger = logger;
            _emailSaver = emailSaver;
        }

        [HttpGet("")]
        public Guid GetData()
        {
            return Guid.NewGuid();
        }

        [HttpPost("")]
        public Guid SendEmailEmail(EmailMessageRequestData emailMessage)
        {
            var message = new EmailMessage(emailMessage.To.ToArray(), emailMessage.From, emailMessage.Topic, emailMessage.Content);
            var newId = _emailSaver.SaveEmail(message);
            return newId;
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailServiceController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<EmailServiceController> _logger;

        public EmailServiceController(ILogger<EmailServiceController> logger)
        {
            _logger = logger;
        }

    }
}

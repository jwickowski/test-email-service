using System;
using System.Collections;

namespace EmailService.Api.Models
{
    public class EmailMessageResponse
    {
        public Guid EmailId { get; set; }
        public  IEnumerable To { get; set; }
        public string From { get;  set; }
        public string Topic { get;  set; }
        public string Content { get;  set; }
        public string EmailStatus { get; set; }
    }
}
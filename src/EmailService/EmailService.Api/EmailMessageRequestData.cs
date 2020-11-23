using System.Collections.Generic;

namespace EmailService.Api
{
    public class EmailMessageRequestData
    {
        public IEnumerable<string> To { get; set; }
        public string From { get;  set; }
        public string Topic { get;  set; }
        public string Content { get;  set; }
    }
}
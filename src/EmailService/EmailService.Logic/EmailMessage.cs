using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailService.Logic
{
    public class EmailMessage
    {
        public EmailMessage(string[] to, string from,string topic, string content)
        {
            _to = to.ToList();
            From = from;
            Content = content;
            Topic = topic;
        }

        private List<string> _to;
        public string[] To
        {
            get
            {
                return _to.ToArray();
            }
        }

        public string From { get; private set; }
        public string Topic { get; private set; }
        public string Content { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Logic
{
    public class EmailMessage
    {
        public EmailMessage(string[] to, string from, string content)
        {
            _to = to.ToList();
            From = from;
            Content = content;
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
        public string Content { get; private set; }
        public int Id { get; private set; }
    }
}

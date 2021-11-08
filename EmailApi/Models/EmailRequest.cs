using System;
using System.Collections.Generic;
using System.Text;

namespace EmailApi.Models
{
    public class EmailRequest
    {
        public string Sender { get; set; }
        public List<string> Recipients { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}

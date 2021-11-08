using System;
using System.Collections.Generic;

#nullable disable

namespace EmailApi.Models
{
    public partial class Inbox
    {
        public string Username { get; set; }
        public int EmailId { get; set; }
        public string Status { get; set; }

        public virtual Email Email { get; set; }
        public virtual User UsernameNavigation { get; set; }
    }
}

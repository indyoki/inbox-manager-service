using System;
using System.Collections.Generic;

#nullable disable

namespace EmailApi.Models
{
    public partial class User
    {
        public User()
        {
            Emails = new HashSet<Email>();
            Inboxes = new HashSet<Inbox>();
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Email> Emails { get; set; }
        public virtual ICollection<Inbox> Inboxes { get; set; }
    }
}

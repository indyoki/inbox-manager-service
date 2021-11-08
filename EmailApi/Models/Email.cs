using System;
using System.Collections.Generic;

#nullable disable

namespace EmailApi.Models
{
    public partial class Email
    {
        public Email()
        {
            Inboxes = new HashSet<Inbox>();
        }

        public int EmailId { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int? LabelId { get; set; }

        public virtual Label Label { get; set; }
        public virtual User SenderNavigation { get; set; }
        public virtual ICollection<Inbox> Inboxes { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace EmailApi.Models
{
    public partial class Label
    {
        public Label()
        {
            Emails = new HashSet<Email>();
        }

        public int LabelId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Email> Emails { get; set; }
    }
}

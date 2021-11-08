using System;
using System.Collections.Generic;
using System.Text;

namespace EmailApi.Models
{
    public class EmailResponse : EmailRequest
    {
        public int Id { get; set; }
        public string LabelDescription { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EmailApi.Models
{
    public enum Status
    {
        Active = 0,
        Deleted = 1
    }
    public enum FilterBy
    {
        Label,
        FromUser
    }
}

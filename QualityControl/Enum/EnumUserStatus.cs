using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualityControl.Enum
{
    public enum EnumUserStatus
    {
        Failed = -4,
        EmailUnauthorized,
        DataRequired,
        Unreviewed,
        Normal
    }
}
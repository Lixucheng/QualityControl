using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualityControl.Enum
{
    public enum EnumUserStatus
    {
        Failed = -3,
        DataRequired = -2,
        Unreviewed,
        Normal
    }
}
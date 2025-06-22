using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Core.Enums
{
    public enum LanguageType
    {
        [Description("简体中文")]
        CN,
        [Description("繁体中文")]
        TW,
        [Description("英文")]
        EN,
    }
}

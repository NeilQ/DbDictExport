using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DbDictExport.Core.Models
{
    public enum NamingRule
    {
        [Description("Named by Camel")]
        Camel=0,

        [Description("Named by Pascal")]
        Pascal=1
    }
}

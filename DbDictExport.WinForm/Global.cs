using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbDictExport.WinForm
{
    public static class Global
    {
        public static string ProviderName { get; set; }

        public static DataBaseType DataBaseType { get; set; }
    }

    public enum DataBaseType
    {
        SqlServer,
        Mysql,
        Postgresql
    }
}

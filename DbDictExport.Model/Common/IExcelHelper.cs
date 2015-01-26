using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using DbDictExport.Model;

namespace DbDictExport.Common
{
    public interface IExcelHelper
    {
        void GenerateWorkbook(List<DbTable> tableList, string fileName);
    }
}

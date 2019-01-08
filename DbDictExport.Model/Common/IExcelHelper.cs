﻿using System.Collections.Generic;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Common
{
    public interface IExcelHelper
    {
        void GenerateWorkbook(IList<Table> tableList, string fileName);
    }
}

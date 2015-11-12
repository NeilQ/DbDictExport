using System.Collections.Generic;

namespace DbDictExport.Core.Common
{
    public interface IExcelHelper
    {
        void GenerateWorkbook(List<DbTable> tableList, string fileName);
    }
}

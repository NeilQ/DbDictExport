using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbDictExport.Common
{
    public static class Constants
    {
        /*
        * the database tree node's Name attribute start with string "db_"
        * the table tree node's Name arrtibute start with string "tb_"
        * the column tree node's Name arrtibute start with string "col_"
        * */
        public const string DATABASE_TREE_NODE_NAME_PREFIX = "db_";
        public const string TABLE_TREE_NODE_NAME_PREFIX = "tb_";
        public const string DATABASE_TEMP_DB_NAME = "tempdb";

        public const string WINDOWS_AUTHENTICATION_TEXT = "Windows Authentication";
        public const string SAVE_FILE_DIALOG_FILTER = "Excel files(*.xlsx)|*.xlsx|Excel files(*.xls)|*.xls;";

        public const string ERROR_CAPTION = "Error";
        public const string EXPORT_CAPTION = "Exporting...";
        public const string VALIDATE_FAIL_CAPTION = "Validate failed";

        public const int TREENODE_ROOT_IMAGE_INDEX = 0;
        public const int TREENODE_DATABASE_IMAGE_INDEX = 1;
        public const int TREENODE_DATATABLE_IMAGE_INDEX = 2;

    }
}

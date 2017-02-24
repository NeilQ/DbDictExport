using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace DbDictExport.Core.Dal
{
    public abstract class SchemaReader
    {
        static Regex rxCleanUp = new Regex(@"[^\w\d_]", RegexOptions.Compiled);

        static string[] cs_keywords =
        {
            "abstract", "event", "new", "struct", "as", "explicit", "null",
            "switch", "base", "extern", "object", "this", "bool", "false", "operator", "throw",
            "break", "finally", "out", "true", "byte", "fixed", "override", "try", "case", "float",
            "params", "typeof", "catch", "for", "private", "uint", "char", "foreach", "protected",
            "ulong", "checked", "goto", "public", "unchecked", "class", "if", "readonly", "unsafe",
            "const", "implicit", "ref", "ushort", "continue", "in", "return", "using", "decimal",
            "int", "sbyte", "virtual", "default", "interface", "sealed", "volatile", "delegate",
            "internal", "short", "void", "do", "is", "sizeof", "while", "double", "lock",
            "stackalloc", "else", "long", "static", "enum", "namespace", "string"
        };

        public abstract List<string> ReadDatabases(DbConnection connection, DbProviderFactory factory);

        public abstract Tables ReadSchema(DbConnection connection, DbProviderFactory factory);
        //  public GeneratedTextTransformation outer;

        /// <summary>
        /// Convert value to Pascal case.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static string ToPascalCase(string value)
        {
            // If there are 0 or 1 characters, just return the string.
            if (value == null) return null;
            if (value.Length < 2) return value.ToUpper();

            // Split the string into words.
            var words = value.Split(
                new[] { '_' },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            var result = "";
            foreach (var word in words)
            {
                result +=
                    word.Substring(0, 1).ToUpper() +
                    word.Substring(1);
            }

            return result;
        }

        protected static Func<string, string> CleanUp = (str) =>
        {
            str = rxCleanUp.Replace(str, "_");

            if (char.IsDigit(str[0]) || cs_keywords.Contains(str))
                str = "@" + str;

            return str;
        };

    }

    public class SqlServerSchemaReader : SchemaReader
    {
        // SchemaReader.ReadSchema
        public override Tables ReadSchema(DbConnection connection, DbProviderFactory factory)
        {
            var result = new Tables();

            _connection = connection;
            _factory = factory;

            var cmd = _factory.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = TABLE_SQL;

            //pull the tables in a reader
            using (cmd)
            {

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var tbl = new Table
                        {
                            Name = rdr["TABLE_NAME"].ToString(),
                            Schema = rdr["TABLE_SCHEMA"].ToString(),
                            IsView =
                                string.Compare(rdr["TABLE_TYPE"].ToString(), "View", StringComparison.OrdinalIgnoreCase) ==
                                0
                        };
                        tbl.CleanName = CleanUp(tbl.Name);
                        tbl.ClassName = Inflector.MakeSingular(tbl.CleanName);
                        result.Add(tbl);
                    }
                }
            }

            foreach (var tbl in result)
            {
                tbl.Columns = LoadColumns(tbl);

                // Mark the primary key
                var pks = GetPK(tbl.Name);
                foreach (var pk in pks)
                {
                    var pkColumn =
                   tbl.Columns.SingleOrDefault(x => x.Name.ToLower().Trim() == pk.ToLower().Trim());
                    if (pkColumn != null)
                    {
                        pkColumn.IsPK = true;
                    }
                }
            }

            return result;
        }

        private DbConnection _connection;
        private DbProviderFactory _factory;

        List<Column> LoadColumns(Table tbl)
        {

            using (var cmd = _factory.CreateCommand())
            {
                cmd.Connection = _connection;
                cmd.CommandText = COLUMN_SQL;

                var p = cmd.CreateParameter();
                p.ParameterName = "@tableName";
                p.Value = tbl.Name;
                cmd.Parameters.Add(p);

                p = cmd.CreateParameter();
                p.ParameterName = "@schemaName";
                p.Value = tbl.Schema;
                cmd.Parameters.Add(p);

                var result = new List<Column>();
                using (IDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Column col = new Column();
                        var lengthStr = rdr["MaxLength"].ToString();
                        col.Name = rdr["ColumnName"].ToString();
                        col.PropertyName = CleanUp(col.Name);
                        col.PropertyType = GetPropertyType(rdr["DataType"].ToString());
                        col.IsNullable = rdr["IsNullable"].ToString() == "YES";
                        col.IsAutoIncrement = ((int)rdr["IsIdentity"]) == 1;
                        if (!string.IsNullOrEmpty(lengthStr))
                        {
                            col.Length = int.Parse(lengthStr);
                        }
                        col.DefaultValue = rdr["DefaultSetting"].ToString();
                        col.DbType = rdr["DataType"].ToString();
                        col.Description = rdr["Description"].ToString();
                        result.Add(col);
                    }
                }

                return result;
            }
        }


        List<string> GetPK(string table)
        {

            string sql = @"SELECT c.name AS ColumnName
                FROM sys.indexes AS i 
                INNER JOIN sys.index_columns AS ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id 
                INNER JOIN sys.objects AS o ON i.object_id = o.object_id 
                LEFT OUTER JOIN sys.columns AS c ON ic.object_id = c.object_id AND c.column_id = ic.column_id
                WHERE (i.is_primary_key = 1) AND (o.name = @tableName)";

            var pks = new List<string>();
            using (var cmd = _factory.CreateCommand())
            {
                cmd.Connection = _connection;
                cmd.CommandText = sql;

                var p = cmd.CreateParameter();
                p.ParameterName = "@tableName";
                p.Value = table;
                cmd.Parameters.Add(p);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            pks.Add(reader["ColumnName"].ToString());
                        }
                    }
                }

            }
            return pks;
        }

        string GetPropertyType(string sqlType)
        {
            string sysType = "string";
            switch (sqlType)
            {
                case "bigint":
                    sysType = "long";
                    break;
                case "smallint":
                    sysType = "short";
                    break;
                case "int":
                    sysType = "int";
                    break;
                case "uniqueidentifier":
                    sysType = "Guid";
                    break;
                case "smalldatetime":
                case "datetime":
                case "datetime2":
                case "date":
                case "time":
                    sysType = "DateTime";
                    break;
                case "datetimeoffset":
                    sysType = "DateTimeOffset";
                    break;
                case "float":
                    sysType = "double";
                    break;
                case "real":
                    sysType = "float";
                    break;
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money":
                    sysType = "decimal";
                    break;
                case "tinyint":
                    sysType = "byte";
                    break;
                case "bit":
                    sysType = "bool";
                    break;
                case "image":
                case "binary":
                case "varbinary":
                case "timestamp":
                    sysType = "byte[]";
                    break;
                case "geography":
                    sysType = "Microsoft.SqlServer.Types.SqlGeography";
                    break;
                case "geometry":
                    sysType = "Microsoft.SqlServer.Types.SqlGeometry";
                    break;
            }
            return sysType;
        }

        public override List<string> ReadDatabases(DbConnection connection, DbProviderFactory factory)
        {
            var result = new List<string>();

            result.AddRange(from DataRow row in connection.GetSchema(SqlClientMetaDataCollectionNames.Databases).Rows select row[0].ToString());

            return result;
        }

        const string TABLE_SQL = @"SELECT *
        FROM  INFORMATION_SCHEMA.TABLES
        WHERE TABLE_TYPE='BASE TABLE' OR TABLE_TYPE='VIEW'
        ORDER BY TABLE_SCHEMA,TABLE_TYPE,TABLE_NAME";

        const string COLUMN_SQL = @"SELECT 
            TABLE_CATALOG AS [Database],
            TABLE_SCHEMA AS Owner, 
            TABLE_NAME AS TableName, 
            COLUMN_NAME AS ColumnName, 
            ORDINAL_POSITION AS OrdinalPosition, 
            COLUMN_DEFAULT AS DefaultSetting, 
            IS_NULLABLE AS IsNullable, DATA_TYPE AS DataType, 
            CHARACTER_MAXIMUM_LENGTH AS MaxLength, 
            DATETIME_PRECISION AS DatePrecision,
            COLUMNPROPERTY(object_id('[' + TABLE_SCHEMA + '].[' + TABLE_NAME + ']'), COLUMN_NAME, 'IsIdentity') AS IsIdentity,
            COLUMNPROPERTY(object_id('[' + TABLE_SCHEMA + '].[' + TABLE_NAME + ']'), COLUMN_NAME, 'IsComputed') as IsComputed,
            s.value as Description
        FROM  INFORMATION_SCHEMA.COLUMNS
        LEFT OUTER JOIN sys.extended_properties s 
        ON 
            s.major_id = OBJECT_ID(INFORMATION_SCHEMA.COLUMNS.TABLE_SCHEMA+'.'+INFORMATION_SCHEMA.COLUMNS.TABLE_NAME) 
            AND s.minor_id = INFORMATION_SCHEMA.COLUMNS.ORDINAL_POSITION 
            AND s.name = 'MS_Description' 
        WHERE TABLE_NAME=@tableName AND TABLE_SCHEMA=@schemaName
        ORDER BY OrdinalPosition ASC";

    }


    public class MySqlSchemaReader : SchemaReader
    {
        // SchemaReader.ReadSchema
        public override List<string> ReadDatabases(DbConnection connection, DbProviderFactory factory)
        {
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SHOW DATABASES;";
            var dbs = new List<string>();
            using (cmd)
            {
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        dbs.Add(rdr["DataBase"].ToString());
                    }
                }
            }
            return dbs;
        }

        public override Tables ReadSchema(DbConnection connection, DbProviderFactory factory)
        {
            var result = new Tables();


            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = TABLE_SQL;

            //pull the tables in a reader
            using (cmd)
            {
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Table tbl = new Table();
                        tbl.Name = rdr["TABLE_NAME"].ToString();
                        tbl.Schema = rdr["TABLE_SCHEMA"].ToString();
                        tbl.IsView = String.Compare(rdr["TABLE_TYPE"].ToString(), "View", StringComparison.OrdinalIgnoreCase) == 0;
                        tbl.CleanName = CleanUp(tbl.Name);
                        tbl.ClassName = Inflector.MakeSingular(tbl.CleanName);
                        result.Add(tbl);
                    }
                }
            }


            //this will return everything for the DB
            var schema = connection.GetSchema("COLUMNS");

            //loop again - but this time pull by table name
            foreach (var item in result)
            {
                item.Columns = new List<Column>();

                //pull the columns from the schema
                var columns = schema.Select("TABLE_NAME='" + item.Name + "'");
                foreach (var row in columns)
                {
                    var lengthStr = row["CHARACTER_MAXIMUM_LENGTH"].ToString();
                    var col = new Column();
                    col.Name = row["COLUMN_NAME"].ToString();
                    col.PropertyName = CleanUp(col.Name);
                    col.PropertyType = GetPropertyType(row);
                    col.IsNullable = row["IS_NULLABLE"].ToString() == "YES";
                    col.IsPK = row["COLUMN_KEY"].ToString() == "PRI";
                    col.IsAutoIncrement = row["extra"].ToString().ToLower().IndexOf("auto_increment", StringComparison.Ordinal) >= 0;
                    if (!string.IsNullOrEmpty(lengthStr))
                    {
                        int length;
                        if (int.TryParse(lengthStr, out length))
                        {
                            col.Length = int.Parse(lengthStr);
                        }
                    }
                    col.DefaultValue = row["COLUMN_DEFAULT"].ToString();
                    col.DbType = row["DATA_TYPE"].ToString();
                    col.Description = row["COLUMN_COMMENT"].ToString();
                    item.Columns.Add(col);
                }
            }

            return result;

        }

        static string GetPropertyType(DataRow row)
        {
            bool bUnsigned = row["COLUMN_TYPE"].ToString().IndexOf("unsigned") >= 0;
            string propType = "string";
            switch (row["DATA_TYPE"].ToString())
            {
                case "bigint":
                    propType = bUnsigned ? "ulong" : "long";
                    break;
                case "int":
                    propType = bUnsigned ? "uint" : "int";
                    break;
                case "smallint":
                    propType = bUnsigned ? "ushort" : "short";
                    break;
                case "guid":
                    propType = "Guid";
                    break;
                case "smalldatetime":
                case "date":
                case "datetime":
                case "timestamp":
                    propType = "DateTime";
                    break;
                case "float":
                    propType = "float";
                    break;
                case "double":
                    propType = "double";
                    break;
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money":
                    propType = "decimal";
                    break;
                case "bit":
                case "bool":
                case "boolean":
                    propType = "bool";
                    break;
                case "tinyint":
                    propType = bUnsigned ? "byte" : "sbyte";
                    break;
                case "image":
                case "binary":
                case "blob":
                case "mediumblob":
                case "longblob":
                case "varbinary":
                    propType = "byte[]";
                    break;

            }
            return propType;
        }

        const string TABLE_SQL = @"
            SELECT * 
            FROM information_schema.tables 
            WHERE (table_type='BASE TABLE' OR table_type='VIEW') AND TABLE_SCHEMA=DATABASE()
            ";

    }

    public class PostgresqlSchemaReader : SchemaReader
    {

        private DbConnection _connection;
        private DbProviderFactory _factory;

        public override List<string> ReadDatabases(DbConnection connection, DbProviderFactory factory)
        {
            var result = new List<string>();
            result.AddRange(from DataRow row in connection.GetSchema(SqlClientMetaDataCollectionNames.Databases).Rows select row[0].ToString());

            return result;
        }

        public override Tables ReadSchema(DbConnection connection, DbProviderFactory factory)
        {
            var result = new Tables();
            _connection = connection;
            _factory = factory;
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = TABLE_SQL;

            //pull the tables in a reader
            using (cmd)
            {
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var tbl = new Table
                        {
                            Name = rdr["table_name"].ToString(),
                            Schema = rdr["table_schema"].ToString(),
                            IsView =
                                string.Compare(rdr["table_type"].ToString(), "View", StringComparison.OrdinalIgnoreCase) ==
                                0
                        };
                        tbl.CleanName = CleanUp(tbl.Name);
                        tbl.ClassName = Inflector.MakeSingular(tbl.CleanName);
                        result.Add(tbl);
                    }
                }
            }

            foreach (var tbl in result)
            {
                tbl.Columns = LoadColumns(tbl);

                var pks = GetPK(tbl.Name);
                foreach (var pk in pks)
                {
                    var pkColumn =
                   tbl.Columns.SingleOrDefault(x => x.Name.ToLower().Trim() == pk.ToLower().Trim());
                    if (pkColumn != null)
                    {
                        pkColumn.IsPK = true;
                    }
                }
            }


            return result;
        }


        List<Column> LoadColumns(Table tbl)
        {

            using (var cmd = _factory.CreateCommand())
            {
                cmd.Connection = _connection;
                cmd.CommandText = COLUMN_SQL;

                var p = cmd.CreateParameter();
                p.ParameterName = "@tableName";
                p.Value = tbl.Name;
                cmd.Parameters.Add(p);

                var result = new List<Column>();
                using (IDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Column col = new Column();
                        col.Name = rdr["column_name"].ToString();
                        col.PropertyName = ToPascalCase(CleanUp(col.Name));
                        col.PropertyType = GetPropertyType(rdr["udt_name"].ToString());
                        col.IsNullable = rdr["is_nullable"].ToString() == "YES";
                        col.IsAutoIncrement = rdr["column_default"].ToString().StartsWith("nextval(");
                        col.DefaultValue = rdr["column_default"].ToString();
                        col.DbType = rdr["udt_name"].ToString();
                        col.Description = rdr["column_comment"].ToString();
                        result.Add(col);
                    }
                }

                return result;
            }
        }

        List<string> GetPK(string table)
        {

            var pks = new List<string>();
            string sql = @"SELECT kcu.column_name 
			FROM information_schema.key_column_usage kcu
			JOIN information_schema.table_constraints tc
			ON kcu.constraint_name=tc.constraint_name
			WHERE lower(tc.constraint_type)='primary key'
			AND kcu.table_name=@tablename";

            using (var cmd = _factory.CreateCommand())
            {
                cmd.Connection = _connection;
                cmd.CommandText = sql;

                var p = cmd.CreateParameter();
                p.ParameterName = "@tableName";
                p.Value = table;
                cmd.Parameters.Add(p);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows) return pks;
                    while (reader.Read())
                    {
                        pks.Add(reader["column_name"].ToString());
                    }
                }

            }

            return pks;
        }

        string GetPropertyType(string sqlType)
        {
            switch (sqlType)
            {
                case "int8":
                case "serial8":
                    return "int";

                case "bool":
                    return "bool";

                case "bytea	":
                    return "byte[]";

                case "float8":
                    return "double";

                case "int4":
                case "serial4":
                    return "int";

                case "money	":
                    return "decimal";

                case "numeric":
                    return "decimal";

                case "float4":
                    return "float";

                case "int2":
                    return "int";

                case "time":
                case "timetz":
                case "timestamp":
                case "timestamptz":
                case "date":
                    return "DateTime";

                default:
                    return "string";
            }
        }



        const string TABLE_SQL = @"
			SELECT table_name, table_schema, table_type
			FROM information_schema.tables 
			WHERE (table_type='BASE TABLE' OR table_type='VIEW')
				AND table_schema NOT IN ('pg_catalog', 'information_schema');
			";

        const string COLUMN_SQL = @"
			SELECT cols.column_name, cols.is_nullable, cols.udt_name, cols.column_default,
                (SELECT pg_catalog.col_description(c.oid, cols.ordinal_position::int)
                 FROM pg_catalog.pg_class c
                 WHERE c.oid = (SELECT ('" + "\"" + @"' || cols.table_name || '" + "\"" + @"')::regclass::oid)
                 AND c.relname = cols.table_name
                ) AS column_comment 
			FROM information_schema.columns cols
			WHERE cols.table_name=@tableName;";
    }
}

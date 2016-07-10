using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace DbDictExport.Core.Dal
{


    public class Poco
    {

        static string ClassPrefix = "";
        static string ClassSuffix = "";
        static string SchemaName = null;
        static bool IncludeViews = false;



        string CheckNullable(Column col)
        {
            string result = "";
            if (col.IsNullable &&
                col.PropertyType != "byte[]" &&
                col.PropertyType != "string" &&
                col.PropertyType != "Microsoft.SqlServer.Types.SqlGeography" &&
                col.PropertyType != "Microsoft.SqlServer.Types.SqlGeometry"
                )
                result = "?";
            return result;
        }


        static string _connectionString = "";


        public static string ConnectionString
        {
            get
            {
                //InitConnectionString();
                return _connectionString;
            }
        }


        static string zap_password(string connectionString)
        {
            var rx = new Regex("password=.*;",
                RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return rx.Replace(connectionString, "password=**zapped**;");
        }

        public static void InitConnectionString(string connectionString)
        {
            _connectionString = connectionString;

        }


        public static Tables LoadTables(string connectionString, string providerName)
        {
            //connBuilder.
            // ConnectionString = connBuilder.ConnectionString;
            InitConnectionString(connectionString);
            DbProviderFactory _factory;
            try
            {
                _factory = DbProviderFactories.GetFactory(providerName);
            }
            catch (Exception e)
            {

                return new Tables();
            }

            try
            {
                Tables result;
                using (var conn = _factory.CreateConnection())
                {
                    conn.ConnectionString = _connectionString;
                    conn.Open();

                    SchemaReader reader = null;

                    if (_factory.GetType().Name == "MySqlClientFactory")
                    {
                        // MySql
                        reader = new MySqlSchemaReader();
                    }
                    else
                    {
                        // Assume SQL Server
                        reader = new SqlServerSchemaReader();
                    }

                    //reader.outer = this;
                    result = reader.ReadSchema(conn, _factory);

                    // Remove unrequired tables/views
                    for (int i = result.Count - 1; i >= 0; i--)
                    {
                        if (SchemaName != null && string.Compare(result[i].Schema, SchemaName, true) != 0)
                        {
                            result.RemoveAt(i);
                            continue;
                        }
                        if (!IncludeViews && result[i].IsView)
                        {
                            result.RemoveAt(i);
                            continue;
                        }
                    }

                    conn.Close();


                    var rxClean =
                        new Regex(
                            "^(Equals|GetHashCode|GetType|ToString|repo|Save|IsNew|Insert|Update|Delete|Exists|SingleOrDefault|Single|First|FirstOrDefault|Fetch|Page|Query)$");
                    foreach (var t in result)
                    {
                        t.ClassName = ClassPrefix + t.ClassName + ClassSuffix;
                        foreach (var c in t.Columns)
                        {
                            c.PropertyName = rxClean.Replace(c.PropertyName, "_$1");

                            // Make sure property name doesn't clash with class name
                            if (c.PropertyName == t.ClassName)
                                c.PropertyName = "_" + c.PropertyName;
                        }
                    }

                    return result;
                }
            }
            catch (Exception x)
            {
                return new Tables();
            }


        }

        public static List<string> LoadDatabases(string connectionString, string providerName)
        {
            InitConnectionString(connectionString);
            DbProviderFactory _factory;
            try
            {
                _factory = DbProviderFactories.GetFactory(providerName);
            }
            catch (Exception e)
            {

                return new List<string>();
            }

            try
            {
                using (var conn = _factory.CreateConnection())
                {
                    conn.ConnectionString = _connectionString;
                    conn.Open();

                    SchemaReader reader;

                    if (_factory.GetType().Name == "MySqlClientFactory")
                    {
                        // MySql
                        reader = new MySqlSchemaReader();
                    }
                    else
                    {
                        // Assume SQL Server
                        reader = new SqlServerSchemaReader();
                    }

                    //reader.outer = this;
                    var result = reader.ReadDatabases(conn, _factory);

                    conn.Close();

                    return result;
                }
            }
            catch (Exception)
            {

                return new List<string>();
            }
        }
    }
}

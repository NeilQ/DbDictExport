using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DbDictExport.Model;

namespace DbDictExport.Dal
{
    /// <summary>
    /// DataAccess class for get the database data.
    /// </summary>
    public sealed class DataAccess
    {
        /// <summary>
        /// Gets a list of database name.
        /// </summary>
        /// <param name="connBuilder">The specified SqlConnectionStringBuilder.</param>
        /// <returns>The list of database name.</returns>
        public static List<string> GetDbNameList(SqlConnectionStringBuilder connBuilder)
        {
            if (connBuilder == null)
            {
                throw new Exception("The connBuilder cannot be null.");
            }
            List<string> list = new List<string>();
            using (var conn = new SqlConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                list.AddRange(from DataRow row in conn.GetSchema(SqlClientMetaDataCollectionNames.Databases).Rows select row[0].ToString());
                conn.Close();
            }
            return list;
        }

        /// <summary>
        /// Gets a dbTable.
        /// </summary>
        /// <param name="connBuilder">The specified SqlConnectionStringBuilder.</param>
        /// <param name="dbName">The specified database name.</param>
        /// <param name="tableName">The specified datatable name.`</param>
        /// <returns></returns>
        public static DbTable GetTableByName(SqlConnectionStringBuilder connBuilder, string dbName, string tableName)
        {
            if (connBuilder == null)
            {
                throw new Exception("The connBuilder cannot be null.");
            }
            if (String.IsNullOrEmpty(dbName))
            {
                throw new Exception("The dbName cannot be null or empty.");
            }
            if (String.IsNullOrEmpty(tableName))
            {
                throw new Exception("The tableName cannot be null.");
            }
            connBuilder.InitialCatalog = dbName;
            DbTable table = null;
            using (SqlConnection conn = new SqlConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT TOP 1 * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE ='BASE TABLE' AND TABLE_NAME=@TableName";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("TableName", tableName));
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    //table
                    table = new DbTable()
                    {
                        Catalog = dr["TABLE_CATALOG"].ToString(),
                        Schema = dr["TABLE_SCHEMA"].ToString(),
                        Name = dr["TABLE_NAME"].ToString(),
                        Type = dr["TABLE_TYPE"].ToString()
                    };
                    //columns
                    table.ColumnList = GetDbColumnList(connBuilder, table);

                }
                conn.Close();
            }
            return table;
        }

        /// <summary>
        /// Gets a list of dbTable.
        /// </summary>
        /// <param name="connBuilder">The specified SqlConnectionStringBuilder.</param>
        /// <param name="dbName">The specified datatable name.</param>
        /// <returns>A list of dbTable, with columns.</returns>
        public static List<DbTable> GetDbTableListWithColumns(SqlConnectionStringBuilder connBuilder, string dbName)
        {
            if (connBuilder == null)
            {
                throw new Exception("The connBuilder cannot be null.");
            }
            if (String.IsNullOrEmpty(dbName))
            {
                throw new Exception("The dbName cannot be null or empry.");
            }
            connBuilder.InitialCatalog = dbName;
            var list = new List<DbTable>();
            using (var conn = new SqlConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE ='BASE TABLE'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    //table
                    DbTable table = new DbTable()
                    {
                        Catalog = dr["TABLE_CATALOG"].ToString(),
                        Schema = dr["TABLE_SCHEMA"].ToString(),
                        Name = dr["TABLE_NAME"].ToString(),
                        Type = dr["TABLE_TYPE"].ToString()
                    };
                    //columns
                    table.ColumnList = GetDbColumnList(connBuilder, table);
                    list.Add(table);
                }
                conn.Close();

            }
            return list.OrderBy(s => s.Name).ToList();
        }

        /// <summary>
        /// Gets a list of datatable name.
        /// </summary>
        /// <param name="connBuilder">The specified SqlConnectionStringBuilder.</param>
        /// <param name="dbName">The specified database name.</param>
        /// <returns>The list of datatable name.</returns>
        public static List<DbTable> GetDbTableNameListWithoutColumns(SqlConnectionStringBuilder connBuilder, string dbName)
        {
            if (connBuilder == null)
            {
                throw new Exception("The connBuilder cannot be null.");
            }
            if (String.IsNullOrEmpty(dbName))
            {
                throw new Exception("The dbName cannot be null or empty.");
            }
            connBuilder.InitialCatalog = dbName;
            var list = new List<DbTable>();
            using (SqlConnection conn = new SqlConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE ='BASE TABLE'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DbTable table = new DbTable()
                    {
                        Catalog = dr["TABLE_CATALOG"].ToString(),
                        Schema = dr["TABLE_SCHEMA"].ToString(),
                        Name = dr["TABLE_NAME"].ToString(),
                        Type = dr["TABLE_TYPE"].ToString()
                    };
                    list.Add(table);
                }
                conn.Close();
            }
            return list.OrderBy(s => s.Name).ToList();
        }

        /// <summary>
        /// Gets a list of DbColumn.
        /// </summary>
        /// <param name="connBuilder">The specified SqlConnectionStringBuilder.</param>
        /// <param name="table">The specified dbTable. <c>table.Name cannot be empty.</c></param>
        /// <returns></returns>
        public static List<DbColumn> GetDbColumnList(SqlConnectionStringBuilder connBuilder, DbTable table)
        {
            if (connBuilder == null)
            {
                throw new Exception("The connBuilder cannot be null.");
            }
            if (table == null)
            {
                throw new Exception("The table cannot be null.");
            }
            else
            {
                if (String.IsNullOrEmpty(table.Name))
                {
                    throw new Exception("The table.Name cannot be null or empty.");
                }
            }
            var list = new List<DbColumn>();
            using (var conn = new SqlConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT  
                                            [TableName]=case when a.colorder=1 then d.name else '' end,  
                                            [TableDescription]=case when a.colorder=1 then isnull(f.value,'') else '' end,  
                                            [ColumnOrder]=a.colorder,  
                                            [Name]=a.name,  
                                            [IsIdentity]=case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '1' else '0' end,  
                                            [PrimaryKey]=case when exists(SELECT 1 FROM sysobjects where xtype='PK' 
                                                and parent_obj=a.id and name in (  
                                                SELECT name FROM sysindexes WHERE indid in(  
                                                SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid  
                                                ))) then '1' else '0' end,  
                                            [Type]=b.name,  
                                            [ByteLength]=a.length,  
                                            [CharactorLength]=COLUMNPROPERTY(a.id,a.name,'PRECISION'),  
                                            --[小数位数]=isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),  
                                            [IsNullable]=case when a.isnullable=1 then '1'else '0' end,  
                                            [DefaultValue]=isnull(e.text,''),  
                                            [Description]=isnull(g.[value],'')  ,
	                                        [ForeignKey]= (SELECT count(ccu.column_name) as have from information_schema.constraint_column_usage ccu inner join information_schema.table_constraints tc ON (ccu.constraint_name = tc.constraint_name) 
					                                          WHERE tc.Constraint_Type = 'FOREIGN KEY' and ccu.table_name = d.name and ccu.column_name = a.name )  
					                                          FROM syscolumns a  
                                        left join systypes b on a.xusertype=b.xusertype  
                                        inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'  
                                        left join syscomments e on a.cdefault=e.id  
                                        left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id  
                                        left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0  
                                        where d.name=@TableName    
                                        order by a.id,a.colorder  ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("TableName", table.Name));
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var column = new DbColumn
                    {
                        DbTable = table,
                        DbType = dr["Type"].ToString(),
                        Name = dr["Name"].ToString(),
                        Order = Int32.Parse(dr["ColumnOrder"].ToString()),
                        DefaultValue = dr["DefaultValue"].ToString(),
                        Description = dr["Description"].ToString(),
                        ForeignKey = dr["ForeignKey"].ToString() != "0",
                        IsIdentity = dr["IsIdentity"].ToString() != "0",
                        IsNullable = dr["IsNullable"].ToString() != "0",
                        Length = Int32.Parse(dr["CharactorLength"].ToString()),
                        PrimaryKey = dr["PrimaryKey"].ToString() != "0"
                    };
                    list.Add(column);
                }
                conn.Close();
            }
            return list;
        }

        /// <summary>
        /// Gets a list of dbColumn.
        /// </summary>
        /// <param name="connBuilder">The specified SqlConnectionStringBuilder.</param>
        /// <param name="tableName">The specified data table name.</param>
        /// <returns>The list of dbColumn.</returns>
        public static List<DbColumn> GetDbColumnList(SqlConnectionStringBuilder connBuilder, string tableName)
        {
            if (connBuilder == null)
            {
                throw new Exception("The connBuilder cannont be null");
            }
            if (String.IsNullOrEmpty(tableName))
            {
                throw new Exception("The tableName cannot be null");
            }
            var list = new List<DbColumn>();
            using (var conn = new SqlConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT  
                                            [TableName]=case when a.colorder=1 then d.name else '' end,  
                                            [TableDescription]=case when a.colorder=1 then isnull(f.value,'') else '' end,  
                                            [ColumnOrder]=a.colorder,  
                                            [Name]=a.name,  
                                            [IsIdentity]=case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '1' else '0' end,  
                                            [PrimaryKey]=case when exists(SELECT 1 FROM sysobjects where xtype='PK' 
                                                and parent_obj=a.id and name in (  
                                                SELECT name FROM sysindexes WHERE indid in(  
                                                SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid  
                                                ))) then '1' else '0' end,  
                                            [Type]=b.name,  
                                            [ByteLength]=a.length,  
                                            [CharactorLength]=COLUMNPROPERTY(a.id,a.name,'PRECISION'),  
                                            --[小数位数]=isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),  
                                            [IsNullable]=case when a.isnullable=1 then '1'else '0' end,  
                                            [DefaultValue]=isnull(e.text,''),  
                                            [Description]=isnull(g.[value],'')  ,
	                                        [ForeignKey]= (SELECT count(ccu.column_name) as have from information_schema.constraint_column_usage ccu inner join information_schema.table_constraints tc ON (ccu.constraint_name = tc.constraint_name) 
					                                          WHERE tc.Constraint_Type = 'FOREIGN KEY' and ccu.table_name = d.name and ccu.column_name = a.name )  
					                                          FROM syscolumns a  
                                        left join systypes b on a.xusertype=b.xusertype  
                                        inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'  
                                        left join syscomments e on a.cdefault=e.id  
                                        left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id  
                                        left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0  
                                        where d.name=@TableName    
                                        order by a.id,a.colorder  ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("TableName", tableName));
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var column = new DbColumn
                    {
                        DbType = dr["Type"].ToString(),
                        Name = dr["Name"].ToString(),
                        Order = Int32.Parse(dr["ColumnOrder"].ToString()),
                        DefaultValue = dr["DefaultValue"].ToString(),
                        Description = dr["Description"].ToString(),
                        ForeignKey = dr["ForeignKey"].ToString() != "0",
                        IsIdentity = dr["IsIdentity"].ToString() != "0",
                        IsNullable = dr["IsNullable"].ToString() != "0",
                        Length = Int32.Parse(dr["CharactorLength"].ToString()),
                        PrimaryKey = dr["PrimaryKey"].ToString() != "0"
                    };
                    list.Add(column);
                }
                conn.Close();
            }
            return list;
        }

        /// <summary>
        /// Gets a dataTable of the top 500 columns.
        /// </summary>
        /// <param name="connBuilder">The specified SqlConnectionStringBuilder.</param>
        /// <param name="table">The specified dbTable.</param>
        /// <returns>A DataTable object of the top 500 column of the specified data table.</returns>
        public static DataTable GetResultSetByDbTable(SqlConnectionStringBuilder connBuilder, DbTable table)
        {
            if (connBuilder == null)
            {
                throw new Exception("The connBuilder cannont be null.");
            }
            if (table == null)
            {
                throw new Exception("The table cannot be null or empty.");
            }
            else
            {
                if (String.IsNullOrEmpty(table.Catalog))
                {
                    throw new Exception("The table.Catalog cannot be null or empty.");
                }
                if (String.IsNullOrEmpty(table.Name))
                {
                    throw new Exception("The table.Name cannot be null or empty.");
                }
            }

            connBuilder.InitialCatalog = table.Catalog;
            var dtResult = new DataTable();
            using (var conn = new SqlConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                var cmd = new SqlCommand
                {
                    CommandText = "SELECT TOP 500 * FROM " + table.Name,
                    Connection = conn
                };
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dtResult);
                conn.Close();
            }
            return dtResult;
        }

    }
}

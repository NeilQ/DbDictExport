using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DbDictExport.WinForm.Model;

namespace DbDictExport.WinForm.Data
{
    public class DataAccess
    {
        /// <summary>
        /// get a list of database name
        /// </summary>
        /// <param name="connBuilder"></param>
        /// <returns></returns>
        public static List<string> GetDbNameList(SqlConnectionStringBuilder connBuilder)
        {
            List<string> list = new List<string>();
            using (SqlConnection conn = new SqlConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                foreach (DataRow row in conn.GetSchema(SqlClientMetaDataCollectionNames.Databases).Rows)
                {
                    list.Add(row[0].ToString());
                }
                conn.Close();
            }
            return list;
        }

        public static DbTable GetTableByName(SqlConnectionStringBuilder connBuilder, string dbName, string tableName)
        {
            connBuilder.InitialCatalog = dbName;
            DbTable table =null ;
            using (SqlConnection conn = new SqlConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT TOP 1 * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE ='BASE TABLE' AND TABLE_NAME=@TableName";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("TableName",tableName));
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

        public static List<DbTable> GetDbTableList(SqlConnectionStringBuilder connBuilder, string dbName)
        {
            connBuilder.InitialCatalog = dbName;
            List<DbTable> list = new List<DbTable>();
            using (SqlConnection conn = new SqlConnection(connBuilder.ConnectionString))
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

        public static List<string> GetDbTableNameList(SqlConnectionStringBuilder connBuilder, string dbName)
        {
            connBuilder.InitialCatalog = dbName;
            List<string> list = new List<string>();
            using (SqlConnection conn = new SqlConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE ='BASE TABLE'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(dr["TABLE_NAME"].ToString());
                }
                conn.Close();
                list.Sort();
            }
            return list;
        }

        public static List<DbColumn> GetDbColumnList(SqlConnectionStringBuilder connBuilder, DbTable table)
        {
            List<DbColumn> list = new List<DbColumn>();
            using (SqlConnection conn = new SqlConnection(connBuilder.ConnectionString))
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
                    DbColumn column = new DbColumn()
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

        public static List<DbColumn> GetDbColumnList(SqlConnectionStringBuilder connBuilder, string tableName)
        {
            List<DbColumn> list = new List<DbColumn>();
            using (SqlConnection conn = new SqlConnection(connBuilder.ConnectionString))
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
                    DbColumn column = new DbColumn()
                    {
                        //DbTable = table,
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

    }
}

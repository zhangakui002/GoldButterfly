using System;
using System.Collections.Generic;
 
using System.Text;
using System.Data.SqlClient;
using System.Data; 

namespace DAL
{
    public class SQLDataProvider:DataProvider
    {
        public SQLDataProvider()
        {
            _DBConnectionString = "";
        }

        string _DBConnectionString { get; set; }

        public SQLDataProvider(string key)
        {
            string connectionString = "";
            try {
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch(Exception er){
                throw new Exception("错误C0001：未发现配置" + key);
            }
            _DBConnectionString = connectionString;
        }

        public override object ExecuteStoreProcedureScalar(System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName)
        {
            if (storedProcedureName == null)
                throw new Exception("存储过程名称不可为Null");
            using (SqlConnection con = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    
                    //设置Sql
                    SqlCommand cmd = new SqlCommand(storedProcedureName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 999;
                    if (parametersInstance != null)
                    {
                        foreach (KeyValuePair<string, object> item in parametersInstance)
                        {
                            SqlParameter parm = new SqlParameter(item.Key, item.Value);
                            cmd.Parameters.Add(parm);
                        }
                    }
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    return cmd.ExecuteScalar();
                }
                catch(Exception er)
                {
                    throw er;
                }
            }
        }

        public override string GetStoreProcName(int storeprocId)
        {
            DataTable dt = ExecuteSqlForDataTable("SELECT name FROM sysobjects WHERE type='p' and id=" + storeprocId.ToString());
            string x = "";
            if (dt.Rows.Count > 0)
                x = dt.Rows[0][0].ToString();
            return x;
        } 

        public override DataSet ExecuteStoreProcedureDataSetWithTableType(string tableTypeKey, DataTable table, System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName)
        {
            DataSet ds = new DataSet(Guid.NewGuid().ToString());
            using (SqlConnection con = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    //设置Sql
                    SqlCommand cmd = new SqlCommand(storedProcedureName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 999;
                    if (parametersInstance != null)
                    {
                        foreach (KeyValuePair<string, object> item in parametersInstance)
                        {
                            SqlParameter parm = new SqlParameter(item.Key, item.Value);
                            cmd.Parameters.Add(parm);
                        }
                    }
                    if (tableTypeKey != null && tableTypeKey.Trim() != "" && table != null)
                    {
                        SqlParameter parm = new SqlParameter(tableTypeKey,SqlDbType.Structured);
                        parm.Value = table;
                        cmd.Parameters.Add(parm);
                    } 
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    return ds;
                }
                catch (Exception er)
                {
                    throw er;
                }
            }
        }

        public override object ExecuteStoreProcedureScalarWithTableType(string tableTypeKey, DataTable table, System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName)
        {
            using (SqlConnection con = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    //设置Sql
                    SqlCommand cmd = new SqlCommand(storedProcedureName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 999;
                    if (parametersInstance != null)
                    {
                        foreach (KeyValuePair<string, object> item in parametersInstance)
                        {
                            SqlParameter parm = new SqlParameter(item.Key, item.Value);
                            cmd.Parameters.Add(parm);
                        }
                    }
                    if (tableTypeKey != null && tableTypeKey.Trim() != "" && table != null)
                    {
                        SqlParameter parm = new SqlParameter(tableTypeKey,SqlDbType.Structured);
                        parm.Value = table;
                        cmd.Parameters.Add(parm);
                    }
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    return cmd.ExecuteScalar();
                }
                catch (Exception er)
                {
                    throw er;
                }
            }
        }

        public override System.Data.DataSet ExecStoredProcedure(System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName)
        {
            using (SqlConnection con = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    //设置Sql
                    SqlCommand cmd = new SqlCommand(storedProcedureName, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parametersInstance != null)
                    {
                        foreach (KeyValuePair<string, object> item in parametersInstance)
                        {
                            SqlParameter parm = new SqlParameter(item.Key, item.Value);
                            cmd.Parameters.Add(parm);
                        }
                    } 
                    DataSet ds = new DataSet();
                    cmd.CommandTimeout = 999;
                    SqlDataAdapter sdap = new SqlDataAdapter(cmd);
                    
                    sdap.Fill(ds);
                    return ds;
                }
                catch (Exception er)
                {
                    throw er;
                }
            }
        }

        public override bool ExecSqlBulkCopyToDBByTransaction(DataTable copying2dbData, string destinationTableName, Dictionary<string, string> sourceToDestinationFieldsMapping)
        {
            //构建多个数据库连接
            SqlConnection connection = null;
            SqlBulkCopy bcp = null;
            SqlTransaction transaction = null;
            try
            {
                connection = new SqlConnection(_DBConnectionString);
                try
                {
                    connection.Open();
                }
                catch
                {
                    throw new Exception("连接数据库失败!请检查连接字符串" + _DBConnectionString);
                }
                transaction = connection.BeginTransaction();
                bcp = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);
                bcp.BatchSize = 500;
                //依次使用数据库连接导入数据
                foreach (KeyValuePair<string, string> kv in sourceToDestinationFieldsMapping)
                {
                    //列映射 
                    bcp.ColumnMappings.Add(kv.Key, kv.Value); 
                }
                bcp.DestinationTableName = destinationTableName; 
                bcp.WriteToServer(copying2dbData);
                transaction.Commit();
                return true;
            }
            catch (Exception er)
            {
                if (transaction != null)
                    transaction.Rollback();
                throw er;
            }
            finally
            {
                connection.Close();
            }
        }

        public override System.Data.DataTable ExecStoreProcedureForGettingTable(System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName)
        {
            using (SqlConnection con = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    //设置Sql
                    SqlCommand cmd = new SqlCommand(storedProcedureName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 999;
                    if (parametersInstance != null)
                    {
                        foreach (KeyValuePair<string, object> item in parametersInstance)
                        {
                            SqlParameter parm = new SqlParameter(item.Key, item.Value);
                            cmd.Parameters.Add(parm);
                        }
                    }
                    DataTable dt = new DataTable(Guid.NewGuid().ToString());
                    SqlDataAdapter sdap = new SqlDataAdapter(cmd);
                    sdap.Fill(dt);
                    return dt;
                }
                catch (Exception er)
                {
                    throw er;
                }
            }
        }

        public override DataTable ExecuteSqlForDataTable(string sql)
        {
            using (SqlConnection con = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    //设置Sql
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 999;
                    DataTable dt = new DataTable(Guid.NewGuid().ToString());
                    SqlDataAdapter sdap = new SqlDataAdapter(cmd);
                    sdap.Fill(dt);
                    return dt;
                }
                catch (Exception er)
                {
                    throw er;
                }
            }
        }

        public override System.Data.DataTable ExecStoreProcedureWithTableType(string tableTypeKey,DataTable table, System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName)
        {
            using (SqlConnection con = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    //设置Sql
                    SqlCommand cmd = new SqlCommand(storedProcedureName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 999;
                    if (parametersInstance != null)
                    {
                        foreach (KeyValuePair<string, object> item in parametersInstance)
                        {
                            SqlParameter parm = new SqlParameter(item.Key, item.Value);
                            cmd.Parameters.Add(parm);
                        }
                    }
                    if (table != null)
                    {
                        SqlParameter parm = new SqlParameter(tableTypeKey, table);
                        cmd.Parameters.Add(parm);
                    }
                    DataTable dt = new DataTable(Guid.NewGuid().ToString());
                    SqlDataAdapter sdap = new SqlDataAdapter(cmd);
                    sdap.Fill(dt);
                    return dt;
                }
                catch (Exception er)
                {
                     throw er;
                }
            }
        }
       
    }
}

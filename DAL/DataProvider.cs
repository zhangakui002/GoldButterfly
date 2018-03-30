using System;
using System.Collections.Generic;
 
using System.Text;
using System.Data; 
using System.Data.SqlClient;

namespace DAL
{
    public abstract class DataProvider
    {
        public static DataProvider Instance(string DBConnection)
        {
            return new SQLDataProvider(DBConnection);
        }
        //}  
        ///// <summary>
        ///// flags:0 mssql;1:mysql
        ///// </summary>
        ///// <param name="DBConnection"></param>
        ///// <param name="flags"></param>
        ///// <returns></returns>
        //public static DataProvider Instance(string DBConnection,int flags)
        //{
        //    ///if(flags==0)
        //    return new SQLDataProvider(DBConnection); 
        //}

        public abstract object ExecuteStoreProcedureScalar(System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName);
        public abstract object ExecuteStoreProcedureScalarWithTableType(string tableTypeKey, DataTable table, System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName);
        public abstract DataSet ExecuteStoreProcedureDataSetWithTableType(string tableTypeKey, DataTable table, System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName);
        public abstract System.Data.DataSet ExecStoredProcedure(System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName);
        public abstract System.Data.DataTable ExecStoreProcedureForGettingTable(System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName);
        public abstract DataTable ExecuteSqlForDataTable(string sql);
        public abstract System.Data.DataTable ExecStoreProcedureWithTableType(string tableTypeKey, DataTable table, System.Collections.Generic.Dictionary<string, object> parametersInstance, string storedProcedureName);
        public abstract bool ExecSqlBulkCopyToDBByTransaction(DataTable copying2dbData, string destinationTableName, Dictionary<string, string> sourceToDestinationFieldsMapping);
        public abstract string GetStoreProcName(int storeprocId);
      
       
    }
}

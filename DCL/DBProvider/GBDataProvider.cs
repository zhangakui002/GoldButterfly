using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCL.DBProvider
{
    public class GBDataProvider : SQLDataProvider
    {

        private static GBDataProvider _Inst;

        public static GBDataProvider Inst
        {
            get
            {
                if (_Inst == null)
                {
                    _Inst = new GBDataProvider();
                }
                return _Inst;
            }
        }

        static string ConfigKey { get { return "GB"; } }
        public GBDataProvider()
            : base(ConfigKey)
        {
        }

        public static DataTable ExecStaticSql(Dictionary<string, object> parms, string sql)
        {
            string FormatedSql = string.Format(sql,parms.Values.ToArray());
            DataTable dt = Inst.ExecuteSqlForDataTable(FormatedSql);
            return dt;
        }

        public static DataTable RegistUser(Dictionary<string, object> parms)
        {
            return ExecStaticSql(parms,GBSql.InsertUser);
        }

        public static DataTable GetUser(Dictionary<string, object> parms)
        {
            return ExecStaticSql(parms, GBSql.SelectUser);
        }

        public static DataTable GetSourceItem(Dictionary<string, object> parms)
        {
            return ExecStaticSql(parms, GBSql.SelectSourceItem);
        }





    }
}

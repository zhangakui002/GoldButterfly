using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCL.DBProvider
{
    public static class GBSql
    {

        public static string SelectUser = @"select * from T_User where userno='{0}'";
        public static string InsertUser = @"insert into T_User (userno,password) values ('{0}','{1}')";
        public static string SelectSourceItem = @"select * from T_Item where sourceNo='{0}'";



    }
}

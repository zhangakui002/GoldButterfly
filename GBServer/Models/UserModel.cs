using DCL.DBProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GBServer.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string UserNo { get; set; }
        public string Password { get; set; }



        public static JsonMessage RegistUser(string userno, string password)
        {
            JsonMessage j = new JsonMessage(); ;
            try
            {
                j.Status = 1;

                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("userno",userno);
                parms.Add("password",password);
                DataTable dt = GBDataProvider.RegistUser(parms);
                j.Data = dt;
            }
            catch (Exception e)
            {
                j.Status = 0;
                j.Message = e.Message;
            }
            return j;
        }

        public static JsonMessage GetUser(string userno)
        {
            JsonMessage j = new JsonMessage(); ;
            try
            {
                j.Status = 1;
                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("userno", userno);
                DataTable dt = GBDataProvider.GetUser(parms);
                j.Data = dt;
            }
            catch (Exception e)
            {
                j.Status = 0;
                j.Message = e.Message;
            }
            return j;
        }


        public static JsonMessage CheckUser(string userno,string password)
        {
            JsonMessage j = new JsonMessage(); ;
            try
            {
                j.Status = 1;
                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("userno", userno);
                parms.Add("password", password);
                DataTable dt = GBDataProvider.RegistUser(parms);
                j.Data = dt;
            }
            catch (Exception e)
            {
                j.Status = 0;
                j.Message = e.Message;
            }
            return j;
        }


    }
}
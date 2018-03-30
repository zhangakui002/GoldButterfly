using DCL.DBProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GBServer.Models
{
    public class SourceNoModel
    {
        public string SourceNo { get; set; }
        public List<ItemModel> Items { get; set; }

        private SourceNoModel() { }

        public static JsonMessage GetResourceNo(string sourceNo)
        {
            JsonMessage j = new JsonMessage(); ;
            try
            {
                j.Status = 1;
                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("sourceNo", sourceNo);
                DataTable dt = GBDataProvider.GetSourceItem(parms);
                SourceNoModel m = new SourceNoModel() { SourceNo = "ABC" };
                m.Items = ItemModel.GetItemsByDt(dt);
                j.Data = dt;

                return j;

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
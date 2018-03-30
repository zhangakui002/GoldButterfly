using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GBServer.Models
{
    public class ItemModel
    {
        public string SourceNo { get; set; }
        public string Spec { get; set; }
        public int Scan { get; set; }
        public string ItemName { get; set; }
        

        public static List<ItemModel> GetItemsByDt(DataTable dt)
        {
            List<ItemModel> l = new List<ItemModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ItemModel i = new ItemModel();
                i.ItemName = dr["itemName"].ToString();
                i.SourceNo = dr["sourceNo"].ToString();
                i.Scan = int.Parse(dr["scan"].ToString());
                i.Spec = dr["spec"].ToString();
                l.Add(i);
            }

            return l;
        }
    }
}
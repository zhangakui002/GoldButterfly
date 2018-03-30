using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBServer.Models
{
    public class StorageModel
    {
        public string Name { get; set; }
        public string Descr { get; set; }


        public static JsonMessage GetSampleStorages()
        {

            JsonMessage j = new JsonMessage(); ;
            try
            {
                StorageModel s1 = new StorageModel() { Name = "s1", Descr = "aaa" };
                StorageModel s2 = new StorageModel() { Name = "s2", Descr = "aaa" };

                List<StorageModel> l = new List<StorageModel>();
                l.Add(s1);
                l.Add(s2);

                j.Data = l;
            }
            catch (Exception e)
            {
                j.Status = 0;
                j.Message = e.Message;
            }

            return j;
        }

        public static JsonMessage GetStorage(string id)
        {
            JsonMessage j = new JsonMessage(); ;
            try
            {
                j.Status = 1;
                j.Data = new StorageModel();
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
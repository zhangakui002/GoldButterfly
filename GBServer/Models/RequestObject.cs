using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBServer.Models
{
    public class RequestObject
    {

        public string RequestName { get; set; }
        public int RequestType { get; set; }
        public Dictionary<string, object> Data { get; set; }




        public string Msg { get; set; }

    }
}
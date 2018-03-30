using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBServer.Models
{
    public class JsonMessage
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
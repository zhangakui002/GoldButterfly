using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GBServer.Models;
using Newtonsoft.Json.Linq;
using DCL.DBProvider;
using Newtonsoft.Json;

namespace GBServer.Controllers
{
    public class SPRequestHandler : IRequestHandler
    {
        public ResponseObject HandleRequest(RequestObject obj)
        {
            return GetReponse(obj.Data, obj.RequestName);
        }

        private ResponseObject GetReponse(Dictionary<string,object> parms,string psName)
        {

            ResponseObject o = new ResponseObject();
            try
            {
                GBDataProvider provider = new GBDataProvider();
                o.Data= provider.ExecStoreProcedureForGettingTable(parms, psName);
                o.Status = 1;
            }
            catch (Exception ex)
            {
                o.Status = 0;
                o.Msg = ex.Message;
            }

            return o;
        }
    }
}
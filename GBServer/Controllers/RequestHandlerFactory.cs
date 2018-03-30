using GBServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBServer.Controllers
{
    public class RequestHandlerFactory
    {

        private RequestHandlerFactory() { }

        private static RequestHandlerFactory _Instance;


        public static RequestHandlerFactory Instance
        {
            get
            {
                if (_Instance==null)
                {
                    _Instance = new RequestHandlerFactory();
                }
                return _Instance;
            }
        }

        public IRequestHandler GetRequestHandlerByReqest(RequestObject obj)
        {
            if (obj.RequestType==0)
            {
                return new SPRequestHandler();
            }

            return new SPRequestHandler();
        }


    }
}
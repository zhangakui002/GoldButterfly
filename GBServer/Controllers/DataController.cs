using GBServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GBServer.Controllers
{
    public class DataController : ApiController
    {

        [HttpGet]
        public IHttpActionResult Get([FromUri]RequestObject obj)
        {
            return Json(HandleRequest(obj));
        }

        [HttpPost]
        public IHttpActionResult Post(RequestObject obj)
        {
            return Json(HandleRequest(obj));
        }

        private ResponseObject HandleRequest(RequestObject obj)
        {

            return RequestHandlerFactory.Instance.GetRequestHandlerByReqest(obj).HandleRequest(obj);
        }
    }
}

using GBServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBServer.Controllers
{
    public interface IRequestHandler
    {
        ResponseObject HandleRequest(RequestObject obj);
    }
}

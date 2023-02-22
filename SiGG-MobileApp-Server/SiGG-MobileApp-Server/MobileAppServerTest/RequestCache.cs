using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.MobileAppServerTest
{
    public class RequestCache
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public List<ServerRequestParameter> Parameters { get; set; }

        public RequestCache()
        {
            Parameters = new List<ServerRequestParameter>();
        }
    }
}

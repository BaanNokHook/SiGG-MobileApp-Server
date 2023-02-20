using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.MobileAppServerClient
{

    public class OperationResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Entity { get; set; }
    }
}

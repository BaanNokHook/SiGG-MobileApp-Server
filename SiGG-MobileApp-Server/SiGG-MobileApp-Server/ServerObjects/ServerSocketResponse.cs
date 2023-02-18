using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerObjects
{
    internal class ServerSocketResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public object entity { get; set; }

        public ServerSocketResponse(int status,
            string message, object entity)
        {
            this.status = status;
            this.message = message;
            this.entity = entity;
        }
    }
}

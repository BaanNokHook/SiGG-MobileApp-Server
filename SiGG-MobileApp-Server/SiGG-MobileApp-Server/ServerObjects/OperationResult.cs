using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerObjects
{
    public struct OperationResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Entity { get; set; }

        public OperationResult(object entity, int status, string message)
        {
            Status = status;
            Entity = entity;
            Message = message;
        }
    }
}

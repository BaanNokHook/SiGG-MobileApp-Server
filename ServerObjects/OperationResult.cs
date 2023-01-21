// Satellite-Communication-Server //

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketAppServer.ServerObjects
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

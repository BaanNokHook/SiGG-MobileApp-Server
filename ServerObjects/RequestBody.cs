// DT Software //

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketAppServer.ServerObjects
{
    public class RequestBody
    {
        [JsonProperty]
        public string Action { get; internal set; }

        [JsonProperty]
        public string Controller { get; internal set; }

        [JsonProperty]
        public List<RequestParameter> Parameters { get; internal set; }
      
        [JsonProperty]
        public string InTo { get; internal set; }

        public RequestBody()
        {

        }
    }
}

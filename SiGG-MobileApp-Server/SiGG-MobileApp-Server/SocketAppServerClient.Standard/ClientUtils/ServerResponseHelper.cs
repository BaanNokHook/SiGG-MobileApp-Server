using SiGG_MobileApp_Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.SocketAppServerClient.Standard.ClientUtils
{
    internal class ServerResponseHelper
    {
        private SocketClientSettings configuration;

        public ServerResponseHelper(SocketClientSettings config)
        {
            configuration = config;
        }


        public byte[] ReceiveBytes(ref Socket clientSocket)
        {
            using (NetworkStream ns = new NetworkStream(clientSocket))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ns.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        public object GetEntityObjectInternal(string entityJson,
             Type entityType = null)
        {
            using (StringReader sr = new StringReader(entityJson))
            {
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    JsonSerializer js = new JsonSerializer();
                    js.ApplyCustomSettings(configuration.SerializerSettings);
                    object entityResult = js.Deserialize(jr, entityType);
                    return entityResult;
                }
            }
        }

        public OperationResult GetResultInternal(string responseContent)
        {
            using (StringReader sr = new StringReader(responseContent))
            {
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    JsonSerializer js = new JsonSerializer();
                    js.ApplyCustomSettings(configuration.SerializerSettings);
                    OperationResult result = js.Deserialize<OperationResult>(jr);
                    return result;
                }
            }
        }

        public ServerResponse ReadResponseInternal(string responseText)
        {
            using (StringReader sr = new StringReader(responseText))
            {
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    JsonSerializer js = new JsonSerializer();
                    js.ApplyCustomSettings(configuration.SerializerSettings);
                    ServerResponse response = js.Deserialize<ServerResponse>(jr);
                    return response;
                }
            }
        }
    }
}

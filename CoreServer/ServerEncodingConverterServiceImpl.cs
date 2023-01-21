// Satellite-Communication-Server //

using SocketAppServer.ManagedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices.CoreServer
{
    internal class ServerEncodingConverterServiceImpl : IEncodingConverterService
    {
        private ICoreServerService coreServer = null;
        public ServerEncodingConverterServiceImpl()
        {
            coreServer = ServiceManager.GetInstance().GetService<ICoreServerService>("realserver");
        }

        public byte[] ConvertToByteArray(string str)
        {
            Encoding encoding = coreServer.GetConfiguration().ServerEncoding;
            return encoding.GetBytes(str);
        }

        public string ConvertToString(byte[] bytes)
        {
            Encoding encoding = coreServer.GetConfiguration().ServerEncoding;
            return encoding.GetString(bytes);
        }
    }
}

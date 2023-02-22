using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.NetFxTestServer
{
    public class BasicServer : IBasicServerController
    {
        public object RunAction(string receivedData, SocketRequest request)
        {
            request.SendData("m                                                                               ");


            return @"<STX>
<L>
<ETX>";
        }
    }
}

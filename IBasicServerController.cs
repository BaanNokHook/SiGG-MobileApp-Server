// Satellite-Communication-Server //

using SocketAppServer.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices
{
    /// <summary>
    /// Basic request processor, works dedicated with a single controller type and does not support advanced features like the standard processor (RequestProcessor)
    /// </summary>
    public interface IBasicServerController : IController
    {
        [ServerAction]
        object RunAction(string receivedData, SocketRequest request);
    }
}

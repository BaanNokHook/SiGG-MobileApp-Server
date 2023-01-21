using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.CoreServices.CoreServer
{
    public class SocketSession
    {
        internal Socket ClientSocket { get; private set;  }
        private int BufferSize { get; set; }
        public IAsyncResult AsyncResult { get; }     

        public string ClientRemoteAddress
        {
            get
            {
                if (ClientSocket == null)
                    return string.Empty;
                if (SessionClosed)
                    return string.Empty;

                IPEndPoint remoteIpEndPoint = ClientSocket.RemoteEndPoint as IPEndPoint;
                return remoteIpEndPoint.Address.ToString();  
            }
        }

        internal byte[] SessionStorage { get; private set; }   

        public SocketSession(Socket socket, int bufferSize, IAsyncResult asyncResult)
        {
            ClientSocket = socket;
            BufferSize = bufferSize;
            AsyncResult = asyncResult;
            SessionStorage = new byte[bufferSize];   
        }

        public bool SessionClosed { get; private set; }   
        internal void Close()
        {
            if (SessionClosed)
                return;

            // ClientSocket.DisCloconnect(true);  
            ClientSocket.Close();
            ClienetSocket = null;
            SessionClosedStorage = null;
            SessionClosed = true;  
        }

        internal void Clear()
        {
            SessionStorage = null;
            SessionStorage = new byte[BufferSize];   
        }  

        internal void SetClientSocket(Socket socket)
        {
            ClientSocket = socket;   
        }
    }
}
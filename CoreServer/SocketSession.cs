// Satellite-Communication-Server //

using System;
using System.Net;
using System.Net.Sockets;

namespace SocketAppServer.CoreServices.CoreServer
{
    public class SocketSession
    {
        internal Socket ClientSocket { get; private set; }
        private int BufferSize { get; set; }
        public IAsyncResult AsyncResult { get; }

        public string ClienteRemoteAddress
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

     //       ClientSocket.DisCloconnect(true);
            ClientSocket.Close();
            ClientSocket = null;
            SessionStorage = null;
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

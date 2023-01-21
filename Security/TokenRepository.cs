// Satellite-Communication-Server //

using SocketAppServer.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocketAppServer.Security
{
    public class TokenRepository
    {
        private static TokenRepository _instance;

        public static TokenRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TokenRepository();
                return _instance;
            }
        }

        public TokenRepository()
        {
            Tokens = new List<ServerToken>();
        }

        internal List<ServerToken> Tokens { get; set; }

        public bool IsValid(string token, ref SocketRequest request)
        {
            ServerToken serverToken = Tokens.FirstOrDefault(t => t.UserToken.Equals(token));
            if (serverToken == null)
                return false;

            if (!serverToken.IsReplicated)
                if (serverToken.RemoteIP != request.RemoteEndPoint.Address.ToString())
                    return false;

            if (serverToken.HasExpired())
            {
                Tokens.Remove(serverToken);
                return false;
            }

            return true;
        }

        public ServerToken GetToken(string token)
        {
            return Tokens.FirstOrDefault(t => t.UserToken.Equals(token));
        }

        public ServerToken AddToken(ServerUser user,
           ref SocketRequest request)
        {
            ServerToken token = new ServerToken(user, ref request);
            Tokens.Add(token);
            return token;
        }

        public ServerToken AddToken(string token, ServerUser user, SocketRequest request)
        {
            ServerToken serverToken = new ServerToken(token, user, request.RemoteEndPoint.ToString());
            Tokens.Add(serverToken);
            return serverToken;
        }

        public void AddReplicatedToken(string token)
        {
            ServerToken serverToken = new ServerToken(token);
            Tokens.Add(serverToken);
        }

        internal ServerToken GetToken(Guid sessionId)
        {
            return Tokens.FirstOrDefault(token => token.SessionId.Equals(sessionId));
        }
    }
}

// Satellite-Communication-Server //

namespace SocketAppServer.Security
{
    public interface IServerUserRepository
    {
        ServerUser Authenticate(string userNameOrEmail, string password);

        void OnSuccessFulAuthentication(string token);
    }
}

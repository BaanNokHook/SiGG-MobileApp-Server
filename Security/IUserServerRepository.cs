// DT Software //

namespace SocketAppServer.Security
{
    public interface IServerUserRepository
    {
        ServerUser Authenticate(string userNameOrEmail, string password);

        void OnSuccessFulAuthentication(string token);
    }
}

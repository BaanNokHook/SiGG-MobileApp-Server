// DT Software //

using SocketAppServer.CoreServices;
using SocketAppServer.ManagedServices;
using SocketAppServer.ServerObjects;

namespace SocketAppServer.Security
{
    public class AuthorizationController : IController
    {
        public ActionResult IsValidToken(string token, SocketRequest request)
        {
            bool valid = TokenRepository.Instance.IsValid(token, ref request);
            string msg = (valid
                ? "You have a valid token"
                : "You have a invalid token");
            return ActionResult.Json(new OperationResult(valid, (valid ? 600 : 500), msg), (valid ? 600 : 500),
                msg);
        }

        public ActionResult Authorize(string user, string password,
            SocketRequest request)
        {
            IServiceManager manager = ServiceManager.GetInstance();
            ISecurityManagementService service = manager.GetService<ISecurityManagementService>();

            var serverUser = service.Authenticate(user, password);
            if (serverUser == null)
                return ActionResult.Json(new OperationResult(string.Empty, 500, "Invalid user"), 500, "Invalid user");
            var token = TokenRepository.Instance.AddToken(serverUser, ref request);

            try
            {
                service.OnSuccessFulAuthentication(token.UserToken);
            }
            catch { }

            return ActionResult.Json(new OperationResult(token.UserToken, 600, "Authorization success. Use this token to authenticate in next requests."));
        }

        [NotListed]
        [ServerAction]
        public void ReplicateToken(string token)
        {
            TokenRepository.Instance.AddReplicatedToken(token);
        }
    }
}

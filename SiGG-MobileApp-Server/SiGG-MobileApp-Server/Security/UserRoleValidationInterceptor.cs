using SiGG_MobileApp_Server.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Security
{
    internal class UserRoleValidationInterceptor : IHandlerInterceptor
{
    public string ControllerName { get { return "*"; } }
    public string ActionName { get { return "*"; } }

    public InterceptorHandleResult PreHandle(SocketRequest socketRequest)
    {
        string controllerName = socketRequest.Controller.GetType().Name;
        string actionName = socketRequest.Action;
        if (controllerName.Equals("AuthorizationController") ||
            controllerName.Equals("ServerInfoController"))
            return new InterceptorHandleResult(false, true, "", "");

        var paramToken = socketRequest.Parameters.FirstOrDefault(p => p.Name.Equals("authorization"));
        if (paramToken == null)
            return new InterceptorHandleResult(true, false, "Unauthorized. Check 'authorization' parameter in request body", false);

        string token = paramToken.Value.ToString();
        if (!TokenRepository.Instance.IsValid(token, ref socketRequest))
            return new InterceptorHandleResult(true, false, "Invalid or expired token. Check 'authorization' parameter in request body", "");

        InterceptorHandleResult result = null;

        var userToken = TokenRepository.Instance.GetToken(token);
        var user = userToken.User;
        if (user.IsRolesAccessControllerEnabled)
        {
            var role = user.Roles.FirstOrDefault(r => r.Controller.Equals(controllerName)
                && (r.ActionName.Equals(actionName) || r.ActionName.Equals("*")));
            if (role == null)
                result = new InterceptorHandleResult(false, true, "Access granted", "");
            else
            {
                if (role.EnableAccess)
                    result = new InterceptorHandleResult(false, true, "Access granted", "");
                else
                    result = new InterceptorHandleResult(true, false, "Access danied", "");
            }
        }
        else
            result = new InterceptorHandleResult(false, true, "Access granted", "");

        userToken.RegisterActivity(new UserActivity(DateTime.Now,
            socketRequest.Controller.GetType().Name, socketRequest.Action,
            result.ResponseSuccess));

        return result;
    }
}
}

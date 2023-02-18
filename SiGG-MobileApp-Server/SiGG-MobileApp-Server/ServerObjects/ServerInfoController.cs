using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerObjects
{
    internal class ServerInfoController : IController
    {
        private IControllerManager controllerManager = null;
        private ICoreServerService coreServer = null;
        private IHardwareServices hardware = null;
        private ITelemetryServicesProvider telemetry = null;
        public ServerInfoController()
        {
            IServiceManager manager = ServiceManager.GetInstance();
            controllerManager = manager.GetService<IControllerManager>();
            coreServer = manager.GetService<ICoreServerService>("realserver");
            hardware = manager.GetService<IHardwareServices>();
            telemetry = manager.GetService<ITelemetryServicesProvider>();
        }

        [ServerAction]
        public List<string> GetActionParameters(string controller,
            string action)
        {
            ControllerRegister register = controllerManager.GetControllerRegister(controller);
            if (register == null)
                return new List<string>();
            MethodInfo method = register.Type.GetMethod(action);
            if (method == null)
                return new List<string>();
            return method
                .GetParameters()
                .Select(p => p.Name)
                .ToList();
        }

        [NotListed]
        public ActionResult Reboot()
        {
            coreServer.Reboot();
            return ActionResult.Json(true, 600, "Ok");
        }

        public ActionResult GetCurrentThreadsCount()
        {
            return ActionResult.Json(new OperationResult(RequestProcessor.ThreadCount, 600, null));
        }

        public ActionResult AverageCPUUsage(double lastMinutes)
        {
            return ActionResult.Json(new OperationResult(hardware.AverageCPUUsage(lastMinutes), 600, null));
        }

        public ActionResult AverageMemoryUsage(double lastMinutes)
        {
            return ActionResult.Json(new OperationResult(hardware.AverageMemoryUsage(lastMinutes), 600, null));
        }

        public ActionResult RequestsSuccessCount(string controllerName, string actionName)
        {
            int count = (telemetry == null
                ? 0
                : telemetry.RequestsSuccessCount(controllerName, actionName));
            return ActionResult.Json(new OperationResult(count, 600, null));
        }

        public ActionResult RequestsErrorsCount(string controllerName, string actionName)
        {
            int count = (telemetry == null
                ? 0
                : telemetry.RequestErrorsCount(controllerName, actionName));
            return ActionResult.Json(new OperationResult(count, 600, null));
        }

        public ActionResult FullServerInfo()
        {
            ServerInfo info = new ServerInfo();
            info.IsLoadBanancingServer = coreServer.IsLoadBalanceEnabled();
            foreach (ControllerRegister controller in controllerManager.GetRegisteredControllers())
                info.ServerControllers.Add(GetControllerInfo(controller.Name));

            return ActionResult.Json(new OperationResult(info, 600, "Server info"));
        }

        public ActionResult DownloadFile(string path)
        {
            return ActionResult.File(path);
        }

        private ControllerInfo GetControllerInfo(string controllerName)
        {
            try
            {
                ControllerRegister register = controllerManager.GetControllerRegister(controllerName);

                Type type = register.Type;

                ControllerInfo info = new ControllerInfo();
                info.ControllerName = controllerName;
                info.ControllerActions = ListActions(controllerName);
                info.ControllerClass = type.FullName;

                return info;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<string> ListActions(string controllerName)
        {
            List<string> result = new List<string>();
            try
            {
                ControllerRegister register = controllerManager.GetControllerRegister(controllerName);

                Type type = register.Type;
                foreach (var method in type.GetMethods())
                {
                    if (method.GetCustomAttribute<NotListed>() != null)
                        continue;

                    if (method.ReturnType == typeof(ActionResult) ||
                        method.GetCustomAttribute<ServerAction>() != null)
                        result.Add(method.Name);
                }

                return result;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }
    }
}

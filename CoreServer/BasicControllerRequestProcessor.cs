// Satellite-Communication-Server //

using SocketAppServer.CoreServices.Logging;
using SocketAppServer.ManagedServices;
using SocketAppServer.ServerObjects;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace SocketAppServer.CoreServices.CoreServer
{
    /// <summary>
    /// Basic request processor, works dedicated with a single controller type and does not support advanced features like the standard processor (RequestProcessor)
    /// </summary>
    public class BasicControllerRequestProcess : AsyncTask<string, string, string>
    {
        private IServiceManager serviceManager = null;
        private ILoggingService logger = null;
        private ICoreServerService coreServer = null;
        private IBasicServerController basicController = null;
        private IEncodingConverterService encoder = null;

        public BasicControllerRequestProcess(RequestPreProcessor preProcessor)
        {
            serviceManager = ServiceManager.GetInstance();
            logger = serviceManager.GetService<ILoggingService>();
            basicController = serviceManager.GetService<IBasicServerController>();
            coreServer = serviceManager.GetService<ICoreServerService>("realserver");
            encoder = serviceManager.GetService<IEncodingConverterService>();

            this.preProcessor = preProcessor;
        }

        private RequestPreProcessor preProcessor = null;

        public override string DoInBackGround(string receivedData)
        {
            try
            {
                SocketRequest req = new SocketRequest(basicController, "RunAction", new System.Collections.Generic.List<RequestParameter>(),
                    preProcessor.clientSocket);

                Stopwatch s = new Stopwatch();
                s.Start();
                object resultObject = basicController.RunAction(receivedData, req);
                s.Stop();

                string resultString = string.Empty;

                if (resultObject != null)
                {
                    resultString = (resultObject.GetType() == typeof(string)
                        ? resultObject.ToString()
                        : JsonConvert.SerializeObject(resultObject, AppServerConfigurator.SerializerSettings));
                }

                byte[] resultBytes = coreServer.GetConfiguration().ServerEncoding.GetBytes(resultString);
                preProcessor.clientSocket.Send(resultBytes);
            }
            catch (Exception ex)
            {
                preProcessor.clientSocket.Send(encoder.ConvertToByteArray($"Error: {ex.Message}"));
                logger.WriteLog($"Basic Server Module Error: {ex.Message}", ServerLogType.ERROR);
            }

            preProcessor.Dispose();
            return string.Empty;
        }

        public override void OnPostExecute(string result)
        {
        }

        public override void OnPreExecute()
        {
        }

        public override void OnProgressUpdate(string progress)
        {
        }
    }
}

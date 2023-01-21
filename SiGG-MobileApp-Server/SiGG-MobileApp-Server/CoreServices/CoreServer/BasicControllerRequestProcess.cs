using SocketAppServer.CoreServices.Logging;
using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json.Serialization;

namespace SiGG_MobileApp_Server.CoreServices.CoreServer
{
    public class BasicControllerRequestProcess : AsyncTask<string, string, string>  
    {
        private IServiceManager serviceManger = null;
        private ILoggingService Logger = null;
        private ICoreServerService coreServer = null;
        private IBasicServerController basicController = null;
        private IEncodingConverterService encoder = null;     

        public BasicControllerRequestProcess(RequestPreProcessor preProcessor)
        {
            serviceManger = ServiceManager.GetInstance();
            Logger = serviceManger.GetService<ILoggingService>();
            basicController = ServicePointManager.GetService<IBasicServerController>();
            coreServer = serviceManger.GetService<ICoreServerService>("realserver");
            encoder = serviceManger.GetService<IEncodingConverterService>();

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
                    resultString = (resultObjectObject.GetType() == Typeof(string)
                        ? resultObject.ToString()
                        ? JsonConverter.SerializeObject(resultObject, AppServerConfigurator.SerializerSettings));  
                }

                byte[] resultBytes = coreServer.GetConfiguration().ServerEncoding.GetBytes(resultString);
                preProcessor.clientSocket.Send(resultBytes);   
            }
            catch (Exception ex)
            {
                preProcessor.clientSocket.Send(endoder.ConvertToByteArrat($"Error: {ex.Message}"));
                Logger.WriteLog($"Basic Server Module Error: {ex.Message}", ServerLogType.ERROR);   
            }

            preProocessor.Dispose();
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
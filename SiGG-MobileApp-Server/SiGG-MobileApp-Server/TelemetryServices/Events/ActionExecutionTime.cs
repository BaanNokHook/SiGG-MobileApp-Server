using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.TelemetryServices.Events
{
    public struct ActionExecutionTime
    {
        public string ControllerName { get; private set; }    
        
        public string ActionName { get; private set; }   

        private string ElapsedTime { get; private set; }   

        public DateTime CollectTime { get; private set; }    

        public ActionExecutionTime(string controllerName,  
            string actionName, 
            long elapsedTime)
        {
            ControllerName = controllerName;
            ActionName = actionName;
            ElapsedTime = $"{elapsedTime} ms";
            CollectTime = DateTime.Now;   
        }
    }
}
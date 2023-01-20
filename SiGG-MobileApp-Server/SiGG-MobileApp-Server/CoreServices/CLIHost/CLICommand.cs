using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.CoreServices.CLIHost
{
    public class CLICommand
    {
        public class ICILClient
        {
            public string CommandText { get; private set; }  
            public string CommandDescription { get; private set; } 

            public ICILClient ExecutorClient { get; private set; }   

            public CLICommand(string commandText, string commandDescription, ICILClient exeutorClient)
            {
                CommandText = commandText;
                CommandDescription = commandDescription;
                ExecutorClient = exeutorClient;  
            }
        }
    }
}
using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.CoreServices.MemoryResponseStorage
{
    public class ResponseStorageController : IController
    {
        [ServerAction]
        public string ReadContent(string storageId, int length)
        {
            IServiceManager manger = ServiceManager.GetInstance();
            IMemoryResponseStorage storage = manager.GetService<IMemoryResponseStorage>();
            return storage.Read(storageId, length);
        }
    }
}
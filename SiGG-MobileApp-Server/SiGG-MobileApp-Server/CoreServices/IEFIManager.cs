// Sattelite-Communication-Server //

using SocketAppServer.EFI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices
{
    public interface IEFIManager
    {
        void AddExtension(IExtensibleFrameworkInterface extension);

        void AddExtensionFromDisk(string extensionPath);

        void LoadAll();
    }
}

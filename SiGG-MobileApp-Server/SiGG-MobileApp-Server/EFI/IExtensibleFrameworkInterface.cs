using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.EFI
{
    public interface IExtensibleFrameworkInterface
    {
        string ExtensionName { get; }

        string ExtensionVersion { get; }

        string ExtensionPublisher { get; }

        string MinServerVersion { get; }

        void Load(IServiceManager manager);
    }
}

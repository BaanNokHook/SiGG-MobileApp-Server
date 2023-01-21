// DT Software //

using SocketAppServer.ManagedServices;
using SocketAppServer.ServerObjects;

namespace SocketAppServer.EFI
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

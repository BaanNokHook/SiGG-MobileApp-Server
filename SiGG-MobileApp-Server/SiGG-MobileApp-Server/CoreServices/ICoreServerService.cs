// Sattelite-Communication-Server //

using SocketAppServer.CoreServices.CoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices
{
    public interface ICoreServerService
    {
        void RunServerStartupTasks();
        void SetConfiguration(ServerConfiguration configuration);
        ServerConfiguration GetConfiguration();
        bool IsServerStarted();
        IReadOnlyCollection<SocketSession> GetCurrentSessions();
        void AcceptCallback(IAsyncResult AR);
        void ReceiveCallback(IAsyncResult AR);
        void Reboot();
        int CurrentThreadsCount();
        void EnableBasicServerProcessorMode(Type basicProccessorType);
        bool IsBasicServerEnabled();
        void Start();
        SocketSession GetSession(Socket clientSocket);
        void RemoveSession(ref SocketSession session);
        bool IsLoadBalanceEnabled();

        string GetServerVersion();

        /// <summary>
        /// Disables standard telemetry services on the server
        /// WARNING!: Disabling telemetry services can bring some extra performance to the server (even if perhaps imperceptible). However it will not be possible to collect metrics to implement improvements in your code
        /// </summary>
        void DisableTelemetryServices();
    }
}

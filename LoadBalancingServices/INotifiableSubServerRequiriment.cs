// Satellite-Communication-Server //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.LoadBalancingServices
{
    public interface INotifiableSubServerRequirement
    {
        SubServer StartNewInstance();

        void StopInstance(SubServer server);
    }
}

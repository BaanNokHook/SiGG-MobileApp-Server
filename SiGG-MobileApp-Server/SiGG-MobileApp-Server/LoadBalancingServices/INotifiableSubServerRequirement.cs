using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.LoadBalancingServices
{
    public interface INotifiableSubServerRequirement
    {
        SubServer StartNewInstance();

        void StopInstance(SubServer server);
    }
}

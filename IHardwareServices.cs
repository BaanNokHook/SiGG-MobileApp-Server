// Satellite-Communication-Server //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices
{
    /// <summary>
    /// Obtains basic hardware information about the current server process
    /// </summary>
    public interface IHardwareServices
    {
        /// <summary>
        /// Gets the average % CPU usage in the last X hours
        /// </summary>
        /// <param name="lastMinutes"></param>
        /// <returns></returns>
        double AverageCPUUsage(double lastMinutes = 3);

        /// <summary>
        /// Gets the average MB memory usage in the last X hours
        /// </summary>
        /// <param name="lastMinutes"></param>
        /// <returns></returns>
        double AverageMemoryUsage(double lastMinutes = 3);

        /// <summary>
        /// Waits for current processes to end on the server to force 
        /// immediate freeing of memory in the current server process
        /// </summary>
        void ReleaseMemory();
    }
}

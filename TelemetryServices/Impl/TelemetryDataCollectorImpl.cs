// Satellite-Communication-Server //

using SocketAppServer.TelemetryServices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.TelemetryServices.Impl
{
    public class TelemetryDataCollectorImpl : ITelemetryDataCollector
    {
        private List<ActionError> errors;
        private List<ActionExecutionTime> actionExecutions;
        private List<DependencyInjectorExecutionTime> dependencyInjectors;
        private List<HardwareUsage> hardwareUsages;
        private List<InterceptorExecutionTime> interceptorExecutions;

        private object lckObj = new object();

        public TelemetryDataCollectorImpl()
        {
            InitializeEventLists();
        }

        private void InitializeEventLists()
        {
            errors = new List<ActionError>(100);
            actionExecutions = new List<ActionExecutionTime>(100);
            dependencyInjectors = new List<DependencyInjectorExecutionTime>(100);
            hardwareUsages = new List<HardwareUsage>(100);
            interceptorExecutions = new List<InterceptorExecutionTime>(100);
        }

        public void Collect(object eventObj)
        {
            lock (lckObj)
            {
                if (eventObj is ActionError)
                    errors.Add((ActionError)eventObj);
                if (eventObj is ActionExecutionTime)
                    actionExecutions.Add((ActionExecutionTime)eventObj);
                if (eventObj is DependencyInjectorExecutionTime)
                    dependencyInjectors.Add((DependencyInjectorExecutionTime)eventObj);
                if (eventObj is HardwareUsage)
                    hardwareUsages.Add((HardwareUsage)eventObj);
                if (eventObj is InterceptorExecutionTime)
                    interceptorExecutions.Add((InterceptorExecutionTime)eventObj);
            }
        }

        public IEnumerable<ActionError> GetActionErros()
        {
            lock (lckObj)
            {
                foreach (var error in errors)
                    yield return error;
                errors.Clear();
                errors = null;
                errors = new List<ActionError>(100);
            }
        }

        public IEnumerable<ActionExecutionTime> GetActionExecutions()
        {
            lock (lckObj)
            {
                foreach (var action in actionExecutions)
                    yield return action;
                actionExecutions.Clear();
                actionExecutions = null;
                actionExecutions = new List<ActionExecutionTime>(100);
            }
        }

        public IEnumerable<DependencyInjectorExecutionTime> GetDependencyInjectors()
        {
            lock (lckObj)
            {
                foreach (var di in dependencyInjectors)
                    yield return di;
                dependencyInjectors.Clear();
                dependencyInjectors = null;
                dependencyInjectors = new List<DependencyInjectorExecutionTime>(100);
            }
        }

        public IEnumerable<HardwareUsage> GetHardwareUsages()
        {
            lock (lckObj)
            {
                foreach (var hw in hardwareUsages)
                    yield return hw;
                hardwareUsages.Clear();
                hardwareUsages = null;
                hardwareUsages = new List<HardwareUsage>(100);
            }
        }

        public IEnumerable<InterceptorExecutionTime> GetInterceptorExecutions()
        {
            lock (lckObj)
            {
                foreach (var intercp in interceptorExecutions)
                    yield return intercp;
                interceptorExecutions.Clear();
                interceptorExecutions = null;
                interceptorExecutions = new List<InterceptorExecutionTime>(100);
            }
        }
    }
}

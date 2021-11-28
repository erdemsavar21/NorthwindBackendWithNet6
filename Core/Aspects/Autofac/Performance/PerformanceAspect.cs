using System;
using System.Diagnostics;
using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging.Log4Net;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Core.Utilities.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Performance
{
    public class PerformanceAspect : MethodInterception
    {
        private int _interval;
        private Stopwatch _stopwatch;
        private LoggerServiceBase _loggerServiceBase;

        public PerformanceAspect(int interval, Type loggerService)
        {
            _interval = interval;
            
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
            
            if (loggerService.BaseType != typeof(LoggerServiceBase))
            {
                throw new System.Exception(AspectMessages.WrongLoggerType);
            }

            _loggerServiceBase = (LoggerServiceBase)Activator.CreateInstance(loggerService);
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval)
            {
               _loggerServiceBase.Warn($"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}--->{_stopwatch.Elapsed.TotalSeconds}");
            }
            _stopwatch.Reset();
        }

    }
}

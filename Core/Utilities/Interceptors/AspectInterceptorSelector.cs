using System;
using System.Reflection;
using Castle.DynamicProxy;
using System.Collections.Generic;
using System.Linq;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Aspects.Autofac.Exception;

namespace Core.Utilities.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public AspectInterceptorSelector()
        {
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();
            var methodAttributes = type.GetMethod(method.Name).GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
            classAttributes.AddRange(methodAttributes);
            classAttributes.Add(new ExceptionLogAspect(typeof(FileLogger))); //tüm aspectler calistiginda exception durumunda loglama yapmasi icin
            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}

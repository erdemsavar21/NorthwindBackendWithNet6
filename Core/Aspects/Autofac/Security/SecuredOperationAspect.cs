using System;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;
using Core.Utilities.Messages;

namespace Core.Aspects.Autofac.Security
{
    public class SecuredOperationAspect : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;
        public SecuredOperationAspect(string roles)
        {
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleclaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roleclaims.Contains(role))
                {
                    return;
                }
            }

            throw new System.Exception(AspectMessages.AuthorizationDenied);
        }
    }
}

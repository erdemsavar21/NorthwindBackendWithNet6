using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.Jwt;
using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.DataAccess.Abstract;
using Northwind.DataAccess.Concrete.EntityFramework;

namespace Northwind.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductManager>().As<IProductService>();
            builder.RegisterType<EfProductDal>().As<IProductDal>();

            builder.RegisterType<EfUserDal>().As<IUserDal>();
            builder.RegisterType<UserManager>().As<IUserService>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()

                }).SingleInstance();
        }
    }
}
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NLS.Framework;
using NLS.ServiceCore;
using System.Reflection;

namespace NLS.Host.Web.Core.Register
{
    /// <summary>
    /// 统一注册IoC
    /// </summary>
    public class IoCRegister
    {
        //通用规则注册
        public static WindsorContainer Register()
        {
            WindsorContainer _container = new WindsorContainer();

            _container.Register(
                Classes.FromAssembly(Assembly.GetAssembly(typeof(IFrameworkDependency)))
                       .IncludeNonPublicTypes()
                       .BasedOn<IFrameworkDependency>()
                       .WithService.Self()
                       .WithService.DefaultInterfaces()
                       .LifestyleTransient()
            );

            _container.Register(
                Classes.FromAssembly(Assembly.GetAssembly(typeof(INLSServiceApplication)))
                       .IncludeNonPublicTypes()
                       .BasedOn<INLSServiceApplication>()
                       .WithService.Self()
                       .WithService.DefaultInterfaces()
                       .LifestyleSingleton()
            );
            return _container;
        }
    }
}
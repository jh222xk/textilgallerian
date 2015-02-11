using System.Linq;
using System.Web.Mvc;
using AdminView;
using Microsoft.Practices.Unity.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(UnityWebActivator), "Prestart")]
[assembly: PostApplicationStartMethod(typeof(UnityWebActivator), "Start")]
[assembly: ApplicationShutdownMethod(typeof(UnityWebActivator), "Shutdown")]

namespace AdminView
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Prestart()
        {
            // Make sure PerRequestLifetimeManaged instaces are disposed on end 
            DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start() 
        {
            var container = UnityConfig.GetConfiguredContainer();

            if (container == null)
            {
                UnityConfig.RegisterComponents();
                container = UnityConfig.GetConfiguredContainer();
            }

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}
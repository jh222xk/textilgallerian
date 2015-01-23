using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace Api
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            // Creates a single DocumentStore during the applications liftime
            container.RegisterInstance(Store);
            // Opens a new session for every request
            container.RegisterType<IDocumentSession>(
                new PerRequestLifetimeManager(),
                new InjectionFactory(c => Store.OpenSession()));
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
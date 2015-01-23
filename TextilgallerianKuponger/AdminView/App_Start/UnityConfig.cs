using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Raven.Client;
using Raven.Client.Document;

namespace AdminView
{
    public static class UnityConfig
    {
        private static readonly IDocumentStore Store = new DocumentStore { ConnectionStringName = "RavenDB" };
        private static UnityContainer _container;

        public static void RegisterComponents()
        {
			_container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            // Creates a single DocumentStore during the applications liftime
            _container.RegisterInstance(Store);
            // Opens a new session for every request
            _container.RegisterType<IDocumentSession>(
                new PerRequestLifetimeManager(),
                new InjectionFactory(c => Store.OpenSession()));
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(_container));
        }

        public static UnityContainer GetConfiguredContainer() {
            return _container;
        }
    }
}
using Domain.Entities;
using Microsoft.Practices.Unity;
using Raven.Client;
using Raven.Client.Document;

namespace AdminView
{
    public static class UnityConfig
    {
        private static readonly IDocumentStore Store = new DocumentStore
        {
            ConnectionStringName = "RavenDB",
            Conventions =
            {
                FindTypeTagName =
                    type => typeof (Coupon).IsAssignableFrom(type) ? "coupons" : null
            }
        };

        private static UnityContainer _container;

        public static void RegisterComponents()
        {
            _container = new UnityContainer();

            // Initialize the database store
            Store.Initialize();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // Creates a single DocumentStore during the applications liftime
            _container.RegisterInstance(Store);
            // Opens a new session for every request
            _container.RegisterType<IDocumentSession>(
                new PerRequestLifetimeManager(),
                new InjectionFactory(c => Store.OpenSession()));
        }

        public static UnityContainer GetConfiguredContainer()
        {
            return _container;
        }
    }
}
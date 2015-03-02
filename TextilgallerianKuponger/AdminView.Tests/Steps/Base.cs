using System.Collections.Generic;
using Domain.Entities;
using NSpec;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using Raven.Client;
using Raven.Client.Embedded;

namespace AdminView.Tests.Steps
{
    public class Base : nspec
    {

        private static IWebDriver _driver;
        private static EmbeddableDocumentStore _documentStore;

        protected static IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                {
                    _documentStore = new EmbeddableDocumentStore
                    {
                        RunInMemory = true,
                        UseEmbeddedHttpServer = true,
                    };
                    _documentStore.Initialize();
                    _documentStore.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists("Coupons");
                    using (var session = _documentStore.OpenSession("Coupons"))
                    {
                        SeedDatabase(session);
                        session.SaveChanges();
                    }
                    _driver = new PhantomJSDriver();
                }
                return _driver;
            }
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver = null;
            _documentStore.Dispose();
        }

        private static void SeedDatabase(IDocumentSession session)
        {
            session.Store(new Role
            {
                Name = "Admin",
                Users = new List<User>
                {
                    new User
                    {
                        IsActive = true,
                        Email = "admin@admin.com",
                        Password = "password",
                    }
                },
                Permissions = new List<Permission>
                {
                    Permission.CanAddCoupons,
                    Permission.CanAddRoles,
                    Permission.CanAddUsers,
                    Permission.CanChangeCoupons,
                    Permission.CanChangeRoles,
                    Permission.CanChangeUsers,
                    Permission.CanDeleteCoupons,
                    Permission.CanDeleteRoles,
                    Permission.CanDeleteUsers,
                    Permission.CanListCoupons,
                    Permission.CanListRoles,
                    Permission.CanListUsers
                },
            });
            session.Store(new Role
            {
                Name = "Editor",
                Users = new List<User>
                {
                    new User
                    {
                        IsActive = true,
                        Email = "editor@admin.com",
                        Password = "password",
                    }
                },
                Permissions = new List<Permission>
                {
                    Permission.CanAddCoupons,
                    Permission.CanChangeCoupons,
                    Permission.CanDeleteCoupons,
                    Permission.CanListCoupons,
                    Permission.CanListRoles,
                    Permission.CanListUsers
                },
            });
            session.Store(new Role
            {
                Name = "Reader",
                Users = new List<User>(),
                Permissions = new List<Permission>
                {
                    Permission.CanListCoupons,
                    Permission.CanListRoles,
                    Permission.CanListUsers
                },
            });
        }
    }
}

using System;
using Domain.Entities;
using Domain.Repositories;
using Raven.Client;
using Raven.Client.Embedded;

namespace Domain.Tests.Helpers
{
    internal class RepositoryFactory : IDisposable
    {
        private CouponRepository _repository;
        private IDocumentSession _session;

        public void Dispose()
        {
            _session.Dispose();
        }

        public CouponRepository Get()
        {
            if (_repository == null)
            {
                // Creates a database in memory that only exists during the test
                var store = new EmbeddableDocumentStore
                {
                    Configuration =
                    {
                        RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true,
                        RunInMemory = true
                    },
                    Conventions =
                    {
                        FindTypeTagName =
                            type => typeof (Coupon).IsAssignableFrom(type) ? "coupons" : null
                    }
                };

                store.Initialize();
                _session = store.OpenSession();
                _repository = new CouponRepository(_session);
            }

            return _repository;
        }
    }
}
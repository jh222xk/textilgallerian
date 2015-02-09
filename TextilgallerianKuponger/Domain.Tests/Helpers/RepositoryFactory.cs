using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Raven.Client;
using Raven.Client.Embedded;

namespace Domain.Tests.Helpers
{
    class RepositoryFactory : IDisposable
    {
        private CouponRepository _repository;
        private IDocumentSession _session;

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
                        RunInMemory = true,
                    },
                    Conventions =
                    {
                        FindTypeTagName =
                            type => typeof(Coupon).IsAssignableFrom(type) ? "coupons" : null
                    }
                };

                store.Initialize();
                _session = store.OpenSession();
                _repository = new CouponRepository(_session);
            }

            return _repository;
        }

        public void Dispose()
        {
            _session.Dispose();
        }
    }
}

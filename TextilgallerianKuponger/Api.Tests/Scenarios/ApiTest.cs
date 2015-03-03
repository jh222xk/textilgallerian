using System;
using System.Collections.Generic;
using Api.Controllers;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Extensions;

namespace Api.Tests.E2E
{
    [TestClass]
    public class ApiTest
    {
        private static EmbeddableDocumentStore _documentStore;
        private IDocumentSession _session;
        private CouponRepository _repository;

        public static CartController CartController { get; private set; }

        [AssemblyInitialize]
        public static void SetupDatabase(TestContext context)
        {
            _documentStore = new EmbeddableDocumentStore
            {
                Configuration =
                {
                    RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true,
                    RunInMemory = true
                },
                Conventions =
                {
                    FindTypeTagName =
                        type => typeof(Coupon).IsAssignableFrom(type) ? "coupons" : null,
                    DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite,
                }
            };
            _documentStore.Initialize();
            using (var session = _documentStore.OpenSession())
            {
                SeedDatabase(session);
                session.SaveChanges();
            }
        }

        [TestInitialize]
        public void SetupController()
        {
            _session = _documentStore.OpenSession();
            _repository = new CouponRepository(_session);
            CartController = new CartController(new CouponService(_repository));
        }

        [TestCleanup]
        public void TeardownController()
        {
            _session.Dispose();
        }

        [AssemblyCleanup]
        public static void TeardownDatabase()
        {
            _documentStore.Dispose();
        }


        private static void SeedDatabase(IDocumentSession session)
        {
            session.Store(new TotalSumPercentageDiscount
            {
                Code = "Inactive",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersUsedBy = new List<Customer>(),
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Active but empty coupon",
                Code = "Active",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersUsedBy = new List<Customer>(),
                CreatedBy = "some@test.com",
                IsActive = true,
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Active but empty coupon that requires a MinPurchase of 1",
                Code = "MinPurchase",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersUsedBy = new List<Customer>(),
                CreatedBy = "some@test.com",
                IsActive = true,
                MinPurchase = 1,
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Active but outdated coupon",
                Code = "Outdated",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                End = DateTime.Now,
                CustomersUsedBy = new List<Customer>(),
                CreatedBy = "some@test.com",
                IsActive = true,
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Active coupon valid for an email",
                Code = "CustomerEmail",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersValidFor = new List<Customer>
                {
                    new Customer
                    {
                        Email = "shopping@customer.com",
                    },
                },
                CustomersUsedBy = new List<Customer>(),
                CreatedBy = "some@test.com",
                IsActive = true,
                UseLimit = 1,
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Active coupon valid for an ssn",
                Code = "CustomerSSN",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersValidFor = new List<Customer>
                {
                    new Customer
                    {
                        SocialSecurityNumber = "600921-1234",
                    },
                },
                CustomersUsedBy = new List<Customer>(),
                CreatedBy = "some@test.com",
                IsActive = true,
                UseLimit = 1,
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Active coupon valid for an disposable code",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersValidFor = new List<Customer>
                {
                    new Customer
                    {
                        CouponCode = "CustomerCode",
                    },
                },
                CustomersUsedBy = new List<Customer>(),
                CreatedBy = "some@test.com",
                IsActive = true,
                UseLimit = 1,
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Used coupon valid for an email",
                Code = "UsedCustomerEmail",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersUsedBy = new List<Customer>
                {
                    new Customer
                    {
                        Email = "shopping@customer.com",
                        CouponUses = 1,
                    },
                },
                CreatedBy = "some@test.com",
                IsActive = true,
                UseLimit = 1,
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Used coupon valid for an ssn",
                Code = "UsedCustomerSSN",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersUsedBy = new List<Customer>
                {
                    new Customer
                    {
                        SocialSecurityNumber = "600921-1234",
                        CouponUses = 1,
                    },
                },
                CreatedBy = "some@test.com",
                IsActive = true,
                UseLimit = 1,
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Used coupon valid for an disposable code",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersUsedBy = new List<Customer>
                {
                    new Customer
                    {
                        CouponCode = "UsedCustomerCode",
                        CouponUses = 1,
                    },
                },
                CreatedBy = "some@test.com",
                IsActive = true,
                UseLimit = 1,
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Used coupon valid for an email two times",
                Code = "UsedCustomerEmail2",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersUsedBy = new List<Customer>
                {
                    new Customer
                    {
                        Email = "shopping@customer.com",
                        CouponUses = 1,
                    },
                },
                CreatedBy = "some@test.com",
                IsActive = true,
                UseLimit = 2,
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Used coupon valid for an ssn two times",
                Code = "UsedCustomerSSN2",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersUsedBy = new List<Customer>
                {
                    new Customer
                    {
                        SocialSecurityNumber = "600921-1234",
                        CouponUses = 1,
                    },
                },
                CreatedBy = "some@test.com",
                IsActive = true,
                UseLimit = 2,
            });
            session.Store(new TotalSumAmountDiscount
            {
                Name = "TestCoupon",
                Description = "Used coupon valid for an disposable code two times",
                Start = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomersUsedBy = new List<Customer>
                {
                    new Customer
                    {
                        CouponCode = "UsedCustomerCode2",
                        CouponUses = 1,
                    },
                },
                CreatedBy = "some@test.com",
                IsActive = true,
                UseLimit = 2,
            });
        }

        protected Cart Check(Cart cart)
        {
            return CartController.Post(cart);
        }

        /// <summary>
        ///     A helper to be used with tests that expects no result to make sure that the coupon
        ///     is actually seeded.
        /// </summary>
        protected void AssertCoupon(String code)
        {
            var coupon = _repository.FindByCode(code);
            if (coupon == null)
            {
                throw new Exception(String.Format("Asserted that a coupon with code {0} exists, but it did not", code));
            }
        }

        /// <summary>
        ///     A helper to be used with tests that expects no result to make sure that the coupon
        ///     is actually seeded.
        /// </summary>
        protected void AssertCouponByCustomerCode(String code)
        {
            var capaign = _repository.FindByCode(code);
            var dispoable = _repository.FindByCustomerCode(code);
            if (capaign != null)
            {
                throw new Exception(String.Format("Asserted that a coupon with capaign code {0} did not exist, but it did", code));
            }
            if (dispoable == null)
            {
                throw new Exception(String.Format("Asserted that a coupon with dispoable code {0} exists, but it did not", code));
            }
        }
    }
}

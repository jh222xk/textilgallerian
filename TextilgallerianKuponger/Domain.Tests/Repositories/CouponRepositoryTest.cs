using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace Domain.Tests.Repositories
{
    [TestClass]
    public class CouponRepositoryTest
    {
        private CouponRepository couponRepository;
        private IDocumentSession session;

        [TestInitialize]
        public void SetUp()
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
                        type => typeof (Coupon).IsAssignableFrom(type) ? "coupons" : null
                }
            };

            store.Initialize();
            session = store.OpenSession();

            couponRepository = new CouponRepository(session);

            // Some demo data to help testing
            couponRepository.Store(
                new BuyProductXRecieveProductY
                {
                    CanBeCombined = true,
                    Code = "XMAS15",
                    CustomersUsedBy = new List<Customer>(),
                    CustomersValidFor = new List<Customer>
                    {
                        new Customer
                        {
                            Email = "email@example.com",
                            SocialSecurityNumber = "831017-2732",
                        },
                        new Customer
                        {
                            Email = "some@email.com",
                            SocialSecurityNumber = "700131-2371",
                        }
                    },
                    Start = DateTime.Now,
                    End = DateTime.Now,
                    RequiredProduct = new List<Product> {new Product {ProductId = "Test"}}
                });

            couponRepository.Store(
                new BuyXProductsPayForYProducts
                {
                    CanBeCombined = true,
                    Code = "XMAS14",
                    CustomersUsedBy = new List<Customer>(),
                    CustomersValidFor = new List<Customer>
                    {
                        new Customer
                        {
                            SocialSecurityNumber = "831017-2732",
                        },
                        new Customer
                        {
                            Email = "some@email.com",
                            SocialSecurityNumber = "700131-2371",
                        }
                    },
                    Start = DateTime.Now,
                    End = DateTime.Now
                });

            couponRepository.Store(
                new TotalSumAmountDiscount
                {
                    CanBeCombined = true,
                    Code = "ihafi7Hsda",
                    CustomersUsedBy = new List<Customer>(),
                    CustomersValidFor = new List<Customer>
                    {
                        new Customer
                        {
                            Email = "other@email.com",
                            SocialSecurityNumber = "831017-2732",
                        },
                        new Customer
                        {
                            SocialSecurityNumber = "700131-2371",
                        },
                    },
                    Start = DateTime.Now,
                    End = DateTime.Now
                });

            couponRepository.SaveChanges();
        }

        [TestCleanup]
        public void TearDown()
        {
            session.Dispose();
        }

        [TestMethod]
        public void TestGettingACouponByCode()
        {
            var coupon = couponRepository.FindByCode("XMAS15");
            Assert.IsInstanceOfType(coupon, typeof (BuyProductXRecieveProductY));
            Assert.AreEqual("XMAS15", coupon.Code);
            Assert.AreEqual(2, coupon.CustomersValidFor.Count);

            foreach (var customer in coupon.CustomersValidFor)
            {
                Assert.IsInstanceOfType(customer, typeof (Customer));
            }
        }

        [TestMethod]
        public void TestGettingCouponsByEmail()
        {
            var coupons = couponRepository.FindByEmail("some@email.com").ToList();
            Assert.AreEqual(2, coupons.Count);

            foreach (var coupon in coupons)
            {
                Assert.IsTrue(
                    coupon.CustomersValidFor.Any(customer => customer.Email == "some@email.com"));
            }

            Assert.IsInstanceOfType(coupons[0], typeof (BuyProductXRecieveProductY));
            Assert.IsInstanceOfType(coupons[1], typeof (BuyXProductsPayForYProducts));
            Assert.AreEqual("XMAS15", coupons[0].Code);
            Assert.AreEqual("XMAS14", coupons[1].Code);
        }

        [TestMethod]
        public void TestGettingCouponsByProduct()
        {
            var coupons = couponRepository.FindByProduct(new Product {ProductId = "Test"}).ToList();
            Assert.AreEqual(1, coupons.Count);

            Assert.AreEqual("Test", coupons[0].Products[0].ProductId);
            Assert.IsInstanceOfType(coupons[0], typeof (BuyProductXRecieveProductY));
            Assert.AreEqual("XMAS15", coupons[0].Code);
        }
    }
}

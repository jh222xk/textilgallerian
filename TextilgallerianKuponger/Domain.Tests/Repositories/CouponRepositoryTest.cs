using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using NSpec;

namespace Domain.Tests.Repositories
{
    [TestClass]
    public class CouponRepositoryTest
    {
        private CouponRepository couponRepository;
        private IDocumentSession session;

        /// <summary>
        /// Setup our configuration and conventions for all of our tests
        /// </summary>
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
                            SocialSecurityNumber = "900131-2371",
                        }
                    },
                    Start = DateTime.Now,
                    End = DateTime.Now,
                    Products = new List<Product> {new Product {ProductId = "Test"}}
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

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestGettingACouponByCode()
        {
            var coupon = couponRepository.FindByCode("XMAS15");

            // We got more than one coupon so check if it's count is equal to 2
            coupon.CustomersValidFor.Count.should_be(2);

            // Check the type of the object
            coupon.should_be_same(typeof (BuyProductXRecieveProductY));

            // Check that our code is correct
            coupon.Code.should_be("XMAS15");

            foreach (var customer in coupon.CustomersValidFor)
            {
                customer.should_be_same(typeof (Customer));
            }
        }

        /// <summary>
        /// Test for getting coupons by a single email
        /// </summary>
        [TestMethod]
        public void TestGettingCouponsByEmail()
        {
            var coupons = couponRepository.FindByEmail("some@email.com").ToList();

            // We got more than one coupon so check if it's count is equal to 2
            coupons.Count.should_be(2);

            foreach (var coupon in coupons)
            {
                coupon.CustomersValidFor.Any(customer => customer.Email == "some@email.com").should_be_true();   
            }

            // Check the type of the objects
            coupons[0].should_be_same(typeof (BuyProductXRecieveProductY));
            coupons[1].should_be_same(typeof (BuyXProductsPayForYProducts));

            // Check that our codes are correct
            coupons[0].Code.should_be("XMAS15");
            coupons[1].Code.should_be("XMAS14");
        }

        /// <summary>
        /// Test for getting coupons by a product, polymorphic relation
        /// </summary>
        [TestMethod]
        public void TestGettingCouponsByProduct()
        {
            var coupons = couponRepository.FindByProduct(new Product {ProductId = "Test"}).ToList();

            // We got only one coupon so check if it's count is equal to 1
            coupons.Count.should_be(1);

            // Check the type of the object
            coupons[0].should_be_same(typeof (BuyProductXRecieveProductY));

            // Check that our code is correct
            coupons[0].Code.should_be("XMAS15");

            // Check that our productId is correct
            coupons[0].Products[0].ProductId.should_be("Test");
        }

        /// <summary>
        /// Test for finding coupons by a social security number
        /// </summary>
        [TestMethod]
        public void TestFindCouponsBySocialSecurityNumber()
        {
            var coupons = couponRepository.FindBySocialSecurityNumber("700131-2371").ToList();

            // We got more than one coupon so check if it's count is equal to 2
            coupons.Count.should_be(2);

            foreach (var coupon in coupons)
            {
                coupon.CustomersValidFor.Any(customer => customer.SocialSecurityNumber == "700131-2371").should_be_true(); 
            }

            // Check the type of the objects
            coupons[0].should_be_same(typeof (BuyProductXRecieveProductY));
            coupons[1].should_be_same(typeof (TotalSumAmountDiscount));

            // Check that our codes are correct
            coupons[0].Code.should_be("XMAS14");
            coupons[1].Code.should_be("ihafi7Hsda");
        }
    }
}

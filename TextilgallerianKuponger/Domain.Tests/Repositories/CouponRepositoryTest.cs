using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec;
using Raven.Client;
using Raven.Client.Embedded;

namespace Domain.Tests.Repositories
{
    [TestClass]
    public class CouponRepositoryTest
    {
        private CouponRepository couponRepository;
        private IDocumentSession session;

        /// <summary>
        ///     Setup our configuration and conventions for all of our tests
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
                    Code = "XMAS15",
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
                    UseLimit = 1,
                    Products = new List<Product> {new Product {ProductId = "Test"}}
                });

            couponRepository.Store(
                new BuyXProductsPayForYProducts
                {
                    Code = "XMAS14",
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
                    UseLimit = 5
                });

            couponRepository.Store(
                new TotalSumAmountDiscount
                {
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
                    }
                });

            couponRepository.SaveChanges();
        }

        [TestCleanup]
        public void TearDown()
        {
            session.Dispose();
        }

        /// <summary>
        ///     Test expected to find 2 results.
        /// </summary>
        [TestMethod]
        public void TestGettingACouponByCode()
        {
            var coupon = couponRepository.FindByCode("XMAS15");

            // We got more than one coupon so check if it's count is equal to 2
            coupon.CustomersValidFor.Count.should_be(2);

            // Check the type of the object
            coupon.should_cast_to<BuyProductXRecieveProductY>();

            // Check that our code is correct
            coupon.Code.should_be("XMAS15");

            // Make sure that our customers are actual customers
            foreach (var customer in coupon.CustomersValidFor)
            {
                customer.should_cast_to<Customer>();
            }
        }

        /// <summary>
        ///     By asking for code that does not exist i expect null result.
        /// </summary>
        [TestMethod]
        public void TestNotGettingACouponByCode()
        {
            // asking for a code that does not exist.
            var coupon = couponRepository.FindByCode("XMAS16");

            // Our repository rules expect null
            coupon.should_be_null();
        }


        /// <summary>
        ///     Test for getting coupons by a single email
        /// </summary>
        [TestMethod]
        public void TestGettingCouponsByEmail()
        {
            var coupons = couponRepository.FindByEmail("some@email.com").ToList();

            // We got more than one coupon so check if it's count is equal to 2
            coupons.Count.should_be(2);

            foreach (var coupon in coupons)
            {
                coupon.CustomersValidFor.Any(customer => customer.Email == "some@email.com")
                      .should_be_true();
            }

            // Check the type of the objects
            coupons[0].should_cast_to<BuyProductXRecieveProductY>();
            coupons[1].should_cast_to<BuyXProductsPayForYProducts>();

            // Check that our codes are correct
            coupons[0].Code.should_be("XMAS15");
            coupons[1].Code.should_be("XMAS14");
        }

        /// <summary>
        ///     Test to check index is 0 if no emails match.
        /// </summary>
        [TestMethod]
        public void TestNotGettingCouponsByEmail()
        {
            var coupons = couponRepository.FindByEmail("unknown@email.com").ToList();

            // We don't have a coupon so check count is equal to 0
            coupons.Count.should_be(0);
        }

        /// <summary>
        ///     Test for getting coupons by a product, polymorphic relation
        /// </summary>
        [TestMethod]
        public void TestGettingCouponsByProduct()
        {
            var coupons =
                couponRepository.FindByProduct(new Product {ProductId = "Test"}).ToList();

            // We got only one coupon so check if it's count is equal to 1
            coupons.Count.should_be(1);

            // Check the type of the object
            coupons[0].should_cast_to<BuyProductXRecieveProductY>();

            // Check that our code is correct
            coupons[0].Code.should_be("XMAS15");

            // Check that our productId is correct
            coupons[0].Products[0].ProductId.should_be("Test");
        }

        /// <summary>
        ///     Method to check we get index of 0 if no procuts match search.
        /// </summary>
        [TestMethod]
        public void TestNotGettingCouponsByProduct()
        {
            var coupons =
                couponRepository.FindByProduct(new Product {ProductId = "noExist"}).ToList();

            // We got no coupon so index should be 0.
            coupons.Count.should_be(0);
        }

        /// <summary>
        ///     Test for finding coupons by a social security number
        /// </summary>
        [TestMethod]
        public void TestFindCouponsBySocialSecurityNumber()
        {
            var coupons =
                couponRepository.FindBySocialSecurityNumber("700131-2371").ToList();

            // We got more than one coupon so check if it's count is equal to 2
            coupons.Count.should_be(2);

            foreach (var coupon in coupons)
            {
                coupon.CustomersValidFor.Any(
                    customer => customer.SocialSecurityNumber == "700131-2371").should_be_true();
            }

            // Check the type of the objects
            coupons[0].should_cast_to<BuyXProductsPayForYProducts>();
            coupons[1].should_cast_to<TotalSumAmountDiscount>();

            // Check that our codes are correct
            coupons[0].Code.should_be("XMAS14");
            coupons[1].Code.should_be("ihafi7Hsda");
        }

        /// <summary>
        ///     Test to asure we get emty list when Query by socialSecurityNumber
        /// </summary>
        [TestMethod]
        public void TestNotFindCouponsBySocialSecurityNumber()
        {
            var coupons =
                couponRepository.FindBySocialSecurityNumber("820709-2371").ToList();

            // We expect to find no matches from database.
            coupons.Count.should_be(0);
        }

        /// <summary>
        ///     Test the store method by updating a Coupon
        /// </summary>
        [TestMethod]
        public void TestUpdateCoupon()
        {
            var coupons = couponRepository.FindByEmail("some@email.com").ToList();
            // 2 saved coupons on this email.
            coupons.Count.should_be(2);

            // hardcoded values before update
            coupons[0].UseLimit.should_be(1);
            coupons[1].UseLimit.should_be(5);

            // update coupon[1] uselimit with new value
            coupons[1].UseLimit = 3;
            // save update
            couponRepository.Store(coupons[1]);

            var updatedCoupons = couponRepository.FindByEmail("some@email.com").ToList();

            // Expect coupon[0].useLimit to have same value, but coupon[1].useLimit to have changed value.
            updatedCoupons[0].UseLimit.should_be(1);
            updatedCoupons[1].UseLimit.should_be(3);
        }

        /// <summary>
        ///     Test the store method by creating a new Coupon
        /// </summary>
        [TestMethod]
        public void TestCreateCoupon()
        {
            var coupon = new TotalSumAmountDiscount
            {
                Code = "NewCode",
            };

            // Store and save coupon.
            couponRepository.Store(coupon);
            couponRepository.SaveChanges();

            // We should be able to retrieve the coupon from the repo now.
            var findcoupon = couponRepository.FindByCode("NewCode");
            findcoupon.should_not_be_null();
        }
    }
}

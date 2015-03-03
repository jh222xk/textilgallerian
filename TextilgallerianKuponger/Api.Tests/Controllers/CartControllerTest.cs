using System.Collections.Generic;
using System.Linq;
using Api.Controllers;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec;
using Raven.Client.Embedded;

namespace Api.Tests.Controllers
{
    [TestClass]
    public class CartControllerTest
    {
        private Cart _cart;
        private CartController _cartController;
        private CouponRepository _couponRepository;

        [TestInitialize]
        public void SetUp()
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
            var session = store.OpenSession();

            _couponRepository = new CouponRepository(session);

            _cartController = new CartController(new CouponService(_couponRepository), _couponRepository);

            _couponRepository.Store(
                Testdata.RandomCoupon(new ValidCoupon
                {
                    Code = "XMAS15"
                }));
            _couponRepository.Store(
                Testdata.RandomCoupon(new ValidCoupon
                {
                    CustomersValidFor = new List<Customer>
                    {
                        new Customer
                        {
                            Email = "test@testmail.com"
                        }
                    }
                }));
            _couponRepository.Store(
                Testdata.RandomCoupon(new ValidCoupon
                {
                    CustomersValidFor = new List<Customer>
                    {
                        new Customer
                        {
                            SocialSecurityNumber = "8888"
                        }
                    }
                }));
            _couponRepository.Store(
                Testdata.RandomCoupon(new InvalidCoupon
                {
                    CustomersValidFor = new List<Customer>
                    {
                        new Customer
                        {
                            Email = "test@testmail.com"
                        }
                    }
                }));
            _couponRepository.Store(
                Testdata.RandomCoupon(new InvalidCoupon
                {
                    CustomersValidFor = new List<Customer>
                    {
                        new Customer
                        {
                            SocialSecurityNumber = "8888"
                        }
                    }
                }));
            _couponRepository.Store(
                Testdata.RandomCoupon(new ValidCoupon
                {
                    Code = "Double",
                    CustomersValidFor = new List<Customer>
                    {
                        new Customer
                        {
                            Email = "double@coupon.com",
                            SocialSecurityNumber = "2222"
                        }
                    }
                }));

            _couponRepository.SaveChanges();
        }

        [TestMethod]
        public void TestThatItHandlesCheckoutsWithoutCoupon()
        {
            _cart = Testdata.RandomCart();

            var result = _cartController.Post(_cart).Discounts.ToList();

            result.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatCouponsAreQueriedByCode()
        {
            _cart = Testdata.RandomCart("XMAS15");

            var result = _cartController.Post(_cart).Discounts.ToList();

            result.Count.should_be(1);
            result[0].Code.should_be("XMAS15");
        }

        [TestMethod]
        public void TestThatCouponsAreQueriedByEmail()
        {
            _cart = Testdata.RandomCart(customerCheckingOut: new Customer
            {
                Email = "test@testmail.com"
            });

            var result = _cartController.Post(_cart).Discounts.ToList();
            // NOTE: Count should be one becouse the other coupon is invalid
            result.Count.should_be(1);
            result[0].CustomersValidFor[0].Email.should_be("test@testmail.com");

            _cart = Testdata.RandomCart(customerCheckingOut: new Customer
            {
                Email = "wrongmail@testmail.com"
            });

            var result2 = _cartController.Post(_cart).Discounts.ToList();
            result2.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatCouponsAreQueriedBySocialSecurityNumber()
        {
            _cart = Testdata.RandomCart(customerCheckingOut: new Customer
            {
                SocialSecurityNumber = "8888"
            });

            var result = _cartController.Post(_cart).Discounts.ToList();
            // NOTE: Count should be one becouse the other coupon is invalid
            result.Count.should_be(1);
            result[0].CustomersValidFor[0].SocialSecurityNumber.should_be("8888");

            _cart = Testdata.RandomCart(customerCheckingOut: new Customer
            {
                SocialSecurityNumber = "8889"
            });

            var result2 = _cartController.Post(_cart).Discounts.ToList();
            result2.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatTheSameCouponIsNotIncludedMultipleTimes()
        {
            _cart = Testdata.RandomCart("Double", new Customer
            {
                Email = "double@coupon.com",
                SocialSecurityNumber = "2222"
            });

            var result = _cartController.Post(_cart).Discounts.ToList();
            // NOTE: Count should be one becouse the other coupon is invalid
            result.Count.should_be(1);
        }
    }
}
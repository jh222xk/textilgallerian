using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Services;
using Domain.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec;

namespace Domain.Tests.Services
{
    [TestClass]
    public class CouponServiceTest
    {
        private CouponService _couponService;
        private RepositoryFactory _repositoryFactory;
        private Customer _validCustomer;
        private Customer _invalidCustomer;
        private Product _freeProduct;
        private Product _invalidProduct;
        private Cart _cart;

        /// <summary>
        ///     Setup our configuration and conventions for all of our tests
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            _repositoryFactory = new RepositoryFactory();
            var repostitory = _repositoryFactory.Get();

            _couponService = new CouponService(repostitory);

            _freeProduct = Testdata.RandomProduct();
            _invalidProduct = Testdata.RandomProduct();
            _validCustomer = Testdata.RandomCustomer();
            _invalidCustomer = Testdata.RandomCustomer();
            _cart = Testdata.EmptyCart();
            _cart.Customer = Testdata.RandomCustomer();
            _cart.Rows.Add(new Row
            {
                Amount = 3,
                Product = Testdata.RandomProduct(),
                ProductPrice = 50
            });

            repostitory.Store(Testdata.RandomCoupon(new ValidCoupon
            {
                Code = "Valid Code"
            }, canBeCombined: true));
            repostitory.Store(Testdata.RandomCoupon(new ValidCoupon
            {
                Code = "Valid Code with customer",
                CustomersValidFor = new List<Customer> { _validCustomer }
            }, canBeCombined: true));
            repostitory.Store(Testdata.RandomCoupon(new InvalidCoupon
            {
                Code = "Valid Coupon"
            }, canBeCombined: true));
            repostitory.Store(Testdata.RandomCoupon(new InvalidCoupon
            {
                CustomersValidFor = new List<Customer> { _validCustomer }
            }, canBeCombined: true));

            repostitory.Store(Testdata.RandomCoupon(new BuyProductXRecieveProductY
            {
                Code = "free product",
                CustomersValidFor = new List<Customer> { _cart.Customer },
                Products = new List<Product> { _cart.Rows.First().Product },
                Buy = 1,
                FreeProduct = _freeProduct,
                Start = DateTime.Now,
                UseLimit = 1000
            }, canBeCombined: true));
            repostitory.Store(Testdata.RandomCoupon(new BuyProductXRecieveProductY
            {
                Code = "Free but uncombineable product",
                CustomersValidFor = new List<Customer> { _cart.Customer },
                Products = new List<Product> { _cart.Rows.First().Product },
                Buy = 1,
                FreeProduct = _invalidProduct,
                Start = DateTime.Now,
                UseLimit = 1000
            }, canBeCombined: false));

            repostitory.Store(Testdata.RandomCoupon(new TotalSumPercentageDiscount
            {
                Code = "20%",
                CustomersValidFor = new List<Customer> { _cart.Customer },
                Percentage = 0.2m,
                Start = DateTime.Now,
                UseLimit = 1000
            }, canBeCombined: true));
            repostitory.Store(Testdata.RandomCoupon(new BuyXProductsPayForYProducts
            {
                Code = "3 for 2",
                Products = new List<Product> { _cart.Rows.First().Product },
                Buy = 3,
                PayFor = 2,
                Start = DateTime.Now,
                UseLimit = 1000
            }, canBeCombined: false, validForEveryone: true));

            repostitory.SaveChanges();
        }

        [TestCleanup]
        public void TearDown()
        {
            _repositoryFactory.Dispose();
        }

        [TestMethod]
        public void TestThatItCanFindCouponsByProvidedCode()
        {
            var cart = Testdata.RandomCart("Valid Code");

            var result = _couponService.FindBestCouponsForCart(cart).Discounts;

            result.Count.should_be(1);
            result[0].Code.should_be("Valid Code");
        }

        [TestMethod]
        public void TestThatItCanNotFindCouponsByInvalidProvidedCode()
        {
            var cart = Testdata.RandomCart("Invalid Code");

            var result = _couponService.FindBestCouponsForCart(cart).Discounts;

            result.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatItCanFindCouponsByCustomerEmail()
        {
            var cart = Testdata.RandomCart(customerCheckingOut: Testdata.RandomCustomer(_validCustomer.Email));

            var result = _couponService.FindBestCouponsForCart(cart).Discounts;

            // Only one becouse the other coupon is invalid
            result.Count.should_be(1);
        }

        [TestMethod]
        public void TestThatItCanNotFindCouponsByInvalidCustomerEmail()
        {
            var cart = Testdata.RandomCart(customerCheckingOut: Testdata.RandomCustomer(_invalidCustomer.Email));

            var result = _couponService.FindBestCouponsForCart(cart).Discounts;

            result.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatItCanFindCouponsByCustomerSsn()
        {
            var cart = Testdata.RandomCart(customerCheckingOut: Testdata.RandomCustomer(socialSecurityNumber: _validCustomer.SocialSecurityNumber));

            var result = _couponService.FindBestCouponsForCart(cart).Discounts;

            // Only one becouse the other coupon is invalid
            result.Count.should_be(1);
        }

        [TestMethod]
        public void TestThatItCanNotFindCouponsByInvalidCustomerSsn()
        {
            var cart = Testdata.RandomCart(customerCheckingOut: Testdata.RandomCustomer(socialSecurityNumber: _invalidCustomer.SocialSecurityNumber));

            var result = _couponService.FindBestCouponsForCart(cart).Discounts;

            result.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatItDoesNotCollectTheSameDiscountMultipleTimes()
        {
            var cart = Testdata.RandomCart("Valid Code with customer", _validCustomer);

            var result = _couponService.FindBestCouponsForCart(cart).Discounts;

            // Only one becouse the other coupon is invalid
            result.Count.should_be(1);
        }

        [TestMethod]
        public void TestThatTheBestOfferIsPicked()
        {
            var result = _couponService.FindBestCouponsForCart(_cart);

            // We only want 2 discounts and 2 rows as the other "free product" coupon isn't combinable
            result.Discounts.Count.should_be(2);
            result.Discounts[0].Code.should_be("free product");
            result.Discounts[1].Code.should_be("20%");
            result.Rows.Count.should_be(2);
            result.Rows[1].Product.should_be(_freeProduct);
        }

        [TestMethod]
        public void TestThatTheBestOfferIsPickedWithCode()
        {
            _cart.CouponCode = "3 for 2";
            var result = _couponService.FindBestCouponsForCart(_cart);

            result.Discounts.Count.should_be(1);
            result.Discounts[0].Code.should_be("3 for 2");
            result.Rows.Count.should_be(1);
        }
    }
}

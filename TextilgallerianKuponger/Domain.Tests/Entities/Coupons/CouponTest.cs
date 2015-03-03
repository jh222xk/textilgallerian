using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec;

namespace Domain.Tests.Entities
{
    [TestClass]
    public class CouponTest
    {
        private Cart _cart;
        private Coupon _coupon;
        private Product _validProduct;

        /// <summary>
        ///     Setup our test data
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            _validProduct = Testdata.RandomProduct();

            _coupon = new BuyXProductsPayForYProducts
            {
                CustomersUsedBy = new List<Customer>(),
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                UseLimit = 5,
                Buy = 3,
                PayFor = 2,
                MinPurchase = 500,
                Products = new List<Product>
                {
                    _validProduct
                },
                IsActive = true
            };

            _cart = new Cart
            {
                Customer = new Customer
                {
                    Email = "user@student.lnu.se",
                    SocialSecurityNumber = "701201-3312"
                },
                Rows = new List<Row>
                {
                    new Row
                    {
                        ProductPrice = 100,
                        Amount = 2,
                        Product = _validProduct
                    },
                    new Row
                    {
                        ProductPrice = 500,
                        Amount = 1,
                        Product = Testdata.RandomProduct()
                    }
                }
            };
        }

        /// <summary>
        ///     Testing that customer is valid if coupon is for everyone and has never been used.
        /// </summary>
        [TestMethod]
        public void TestCouponValidIfForEveryoneAndNeverUsed()
        {
            _coupon.IsValidFor(_cart).should_be_true();
        }

        /// <summary>
        ///     Testing that customer is valid if in the CustomersValidFor-list.
        /// </summary>
        [TestMethod]
        public void TestCouponIsValidIfCustomerIsInValidCustomerList()
        {
            _coupon.CustomersValidFor = new List<Customer>
            {
                new Customer
                {
                    Email = "user@student.lnu.se",
                    SocialSecurityNumber = "701201-3312"
                },
                new Customer
                {
                    Email = "some@email.com",
                    SocialSecurityNumber = "900131-2371"
                }
            };


            _coupon.IsValidFor(_cart).should_be_true();
        }

        /// <summary>
        ///     Testing that customer is valid if coupon has already been used but not exceeded uselimit
        /// </summary>
        [TestMethod]
        public void TestCouponValidIfUseLimitNotReached()
        {
            _coupon.CustomersUsedBy = new List<Customer>
            {
                new Customer
                {
                    CouponUses = 1,
                    Email = "user@student.lnu.se",
                    SocialSecurityNumber = "701201-3312"
                },
                new Customer
                {
                    Email = "some@email.com",
                    SocialSecurityNumber = "900131-2371"
                }
            };

            _coupon.IsValidFor(_cart).should_be_true();
        }

        /// <summary>
        ///     Test if customer is NOT valid if uselimit has been reached.
        /// </summary>
        [TestMethod]
        public void TestCouponNotValidIfUseLimitReachedForCustomer()
        {
            _coupon.CustomersUsedBy = new List<Customer>
            {
                new Customer
                {
                    CouponUses = 5,
                    Email = "user@student.lnu.se",
                    SocialSecurityNumber = "701201-3312"
                }
            };

            _coupon.IsValidFor(_cart).should_be_false();
        }

        /// <summary>
        ///     Test if customer is NOT valid if in CustomersValidFor-list but uselimit has been reached.
        /// </summary>
        [TestMethod]
        public void TestCouponNotValidIfUseLimitReachedForValidCustomer()
        {
            _coupon.CustomersValidFor = new List<Customer>
            {
                new Customer
                {
                    CouponUses = 5,
                    Email = "user@student.lnu.se",
                    SocialSecurityNumber = "701201-3312"
                },
                new Customer
                {
                    Email = "some@email.com",
                    SocialSecurityNumber = "900131-2371"
                }
            };

            _coupon.IsValidFor(_cart).should_be_false();
        }

        /// <summary>
        ///     Test so customer is not valid if not in the CustomersValidFor-list
        /// </summary>
        [TestMethod]
        public void TestCustomerNotValidIfNotInCustomersValidForList()
        {
            _coupon.CustomersValidFor = new List<Customer>
            {
                new Customer
                {
                    Email = "some@email.com",
                    SocialSecurityNumber = "900131-2371"
                }
            };

            _coupon.IsValidFor(_cart).should_be_false();
        }

        /// <summary>
        ///     Test so outdated coupon is not valid.
        /// </summary>
        [TestMethod]
        public void TestOldCouponNotValid()
        {
            //yesterday
            _coupon.End = DateTime.Now.AddDays(-1);

            _coupon.IsValidFor(_cart).should_be_false();
        }

        /// <summary>
        ///     Test so outdated coupon is not valid even if customer is in CustomersValidFor-list
        /// </summary>
        [TestMethod]
        public void TestOldCouponNotValidForCustomerInCustomersValidForList()
        {
            _coupon.CustomersValidFor = new List<Customer>
            {
                new Customer
                {
                    Email = "some@email.com",
                    SocialSecurityNumber = "900131-2371"
                }
            };
            //yesterday
            _coupon.End = DateTime.Now.AddDays(-1);

            _coupon.IsValidFor(_cart).should_be_false();
        }

        /// <summary>
        ///     Test so CustomerUsedBy have reached the UseLimit for this discount
        /// </summary>
        [TestMethod]
        public void TestThatCustomerUsedByUseLimitReachedShouldBeNotValid()
        {
            _coupon.CustomersUsedBy = new List<Customer>
            {
                new Customer
                {
                    Email = "user@student.lnu.se",
                    SocialSecurityNumber = "701201-3312",
                    CouponUses = 2
                }
            };

            _coupon.UseLimit = 1;

            _coupon.IsValidFor(_cart).should_be_false();
        }

        [TestMethod]
        public void TestThatMinPurchaseMustBeExceeded()
        {
            _cart.Rows = new List<Row>
            {
                    new Row
                    {
                        ProductPrice = 100,
                        Amount = 2,
                        Product = _validProduct
                    },
            };

            _coupon.IsValidFor(_cart).should_be_false();
        }
    }
}
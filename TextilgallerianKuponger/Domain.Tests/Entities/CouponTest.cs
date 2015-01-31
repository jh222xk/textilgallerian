using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using System.Collections.Generic;
using NSpec;

namespace Domain.Tests.Entities
{
    [TestClass]
    public class CouponTest
    {
        private Cart cart;
        private Coupon coupon;

        /// <summary>
        /// Setup our test data
        /// </summary>

        [TestInitialize]
        public void SetUp()
        {

            coupon = new BuyXProductsPayForYProducts
            {
                CanBeCombined = false,
                Code = "XMAS15",
                CustomersUsedBy = new List<Customer>(),
                CustomersValidFor = new List<Customer>(),
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                UseLimit = 5,
                Buy = 3,
                PayFor = 2
            };


            cart = new Cart
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
                        NumberOfProducts = 2,
                        Product = new Product
                        {
                            ProductId = "My-Test-Product",
                            Name = "A wonderful product"
                        }
                    },
                    new Row
                    {
                        ProductPrice = 500,
                        NumberOfProducts = 1,
                        Product = new Product
                        {
                            ProductId = "My-Test-Product-2",
                            Name = "A not so wonderful product"
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Testing that customer is valid if coupon is for everyone and has never been used.
        /// </summary>

        [TestMethod]
        public void TestCouponValidIfForEveryoneAndNeverUsed()
        {
            coupon.IsValidFor(cart).should_be_true();
        }


        /// <summary>
        /// Testing that customer is valid if in the CustomersValidFor-list.
        /// </summary>

        [TestMethod]
        public void TestCouponIsValidIfCustomerIsInValidCustomerList()
        {
            coupon.CustomersValidFor = new List<Customer>
                {
                    new Customer
                    {
                        Email = "user@student.lnu.se",
                        SocialSecurityNumber = "701201-3312",
                    },
                    new Customer
                    {
                        Email = "some@email.com",
                        SocialSecurityNumber = "900131-2371",
                    }
                };


            coupon.IsValidFor(cart).should_be_true();
        }


        /// <summary>
        /// Testing that customer is valid if coupon has already been used but not exceeded uselimit
        /// </summary>

        [TestMethod]
        public void TestCouponValidIfUseLimitNotReached()
        {
            coupon.CustomersUsedBy = new List<Customer>
                {
                    new Customer
                    {
                        CouponUses = 1,
                        Email = "user@student.lnu.se",
                        SocialSecurityNumber = "701201-3312",
                    },
                    new Customer
                    {
                        Email = "some@email.com",
                        SocialSecurityNumber = "900131-2371",
                    }
                };

            coupon.IsValidFor(cart).should_be_true();
        }

        /// <summary>
        /// Test if customer is NOT valid if uselimit has been reached.
        /// </summary>

        [TestMethod]
        public void TestCouponNotValidIfUseLimitReachedForCustomer()
        {
            coupon.CustomersUsedBy = new List<Customer>
                {
                    new Customer
                    {
                        CouponUses = 5,
                        Email = "user@student.lnu.se",
                        SocialSecurityNumber = "701201-3312"
                    }
                };

            coupon.IsValidFor(cart).should_be_false();
        }


        /// <summary>
        /// Test if customer is NOT valid if in CustomersValidFor-list but uselimit has been reached.
        /// </summary>

        [TestMethod]
        public void TestCouponNotValidIfUseLimitReachedForValidCustomer()
        {
            coupon.CustomersValidFor = new List<Customer>
                {
                    new Customer
                    {
                        CouponUses = 5,
                        Email = "user@student.lnu.se",
                        SocialSecurityNumber = "701201-3312",
                    },
                    new Customer
                    {
                        Email = "some@email.com",
                        SocialSecurityNumber = "900131-2371",
                    }
                };

            coupon.IsValidFor(cart).should_be_false();
        }


        /// <summary>
        /// Test so customer is not valid if not in the CustomersValidFor-list
        /// </summary>
        [TestMethod]
        public void TestCustomerNotValidIfNotInCustomersValidForList()
        {
            coupon.CustomersValidFor = new List<Customer>
                {
                    new Customer
                    {
                        Email = "some@email.com",
                        SocialSecurityNumber = "900131-2371"
                    }
                };

            coupon.IsValidFor(cart).should_be_false();
        }

        /// <summary>
        /// Test so outdated coupon is not valid.
        /// </summary>

        [TestMethod]
        public void TestOldCouponNotValid()
        {
            //yesterday
            coupon.End = DateTime.Now.AddDays(-1);

            coupon.IsValidFor(cart).should_be_false();
        }


        /// <summary>
        /// Test so outdated coupon is not valid even if customer is in CustomersValidFor-list
        /// </summary>

        [TestMethod]
        public void TestOldCouponNotValidForCustomerInCustomersValidForList()
        {
            coupon.CustomersValidFor = new List<Customer>
                {
                    new Customer
                    {
                        Email = "some@email.com",
                        SocialSecurityNumber = "900131-2371"
                    }
                };
            //yesterday
            coupon.End = DateTime.Now.AddDays(-1);

            coupon.IsValidFor(cart).should_be_false();
        }

    }
}

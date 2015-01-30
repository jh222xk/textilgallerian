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
        private Coupon coupon2;
        private Coupon coupon3;
        private Coupon coupon4;
        private Coupon coupon5;

        /// <summary>
        /// Setup our test data
        /// </summary>

        // TODO: Needs refactoring
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

            coupon2 = new BuyXProductsPayForYProducts
            {
                CanBeCombined = false,
                Code = "XMAS15",
                CustomersUsedBy = new List<Customer>(),
                CustomersValidFor = new List<Customer>
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
                },
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                UseLimit = 5,
                Buy = 3,
                PayFor = 2
            };

            coupon3 = new BuyXProductsPayForYProducts
            {
                CanBeCombined = false,
                Code = "XMAS15",
                CustomersUsedBy = new List<Customer>
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
                },
                CustomersValidFor = new List<Customer>(),
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                UseLimit = 5,
                Buy = 3,
                PayFor = 2
            };

            coupon4 = new BuyXProductsPayForYProducts
            {
                CanBeCombined = false,
                Code = "XMAS15",
                CustomersUsedBy = new List<Customer>
                {
                    new Customer
                    {
                        CouponUses = 5,
                        Email = "user@student.lnu.se",
                        SocialSecurityNumber = "701201-3312"
                    }
                },
                CustomersValidFor = new List<Customer>(),
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                UseLimit = 5,
                Buy = 3,
                PayFor = 2
            };

            coupon5 = new BuyXProductsPayForYProducts
            {
                CanBeCombined = false,
                Code = "XMAS15",
                CustomersUsedBy = new List<Customer>(),
                CustomersValidFor = new List<Customer>
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
                },
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
        /// 
        /// </summary>

        // TODO: Need better names and comments
        [TestMethod]
        public void TestCanCheckIsValidFor()
        {
            coupon.IsValidFor(cart).should_be_true();
        }


        /// <summary>
        /// 
        /// </summary>

        // TODO: Need better names and comments
        [TestMethod]
        public void TestCanCheckIfCouponIsValidForCustomerList()
        {
            coupon2.IsValidFor(cart).should_be_true();
        }

        /// <summary>
        /// 
        /// </summary>

        // TODO: Need better names and comments
        [TestMethod]
        public void TestCanCheckIfCouponIsNOTValidForCustomerList()
        {
            coupon5.IsValidFor(cart).should_be_false();
        }

        /// <summary>
        /// 
        /// </summary>

        // TODO: Need better names and comments
        [TestMethod]
        public void TestCanCheckIfCouponIsValidForUsedByCustomerList()
        {
            coupon3.IsValidFor(cart).should_be_true();
        }

        /// <summary>
        /// 
        /// </summary>

        // TODO: Need better names and comments
        [TestMethod]
        public void TestCanCheckIfCouponIsNOTValidForUsedByCustomerList()
        {
            coupon4.IsValidFor(cart).should_be_false();
        }
    }
}

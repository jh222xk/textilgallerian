using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec;

namespace Domain.Tests.Entities
{
    [TestClass]
    public class BuyXProductsPayForYProductsTest
    {
        private Cart _cart;
        private Coupon _coupon;
        private Product _freeProduct;
        private Product _validProduct;

        /// <summary>
        ///     Setup our test data
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            _freeProduct = Testdata.RandomProduct();
            _validProduct = Testdata.RandomProduct();

            _coupon = new BuyXProductsPayForYProducts
            {
                CanBeCombined = false,
                Code = "XMAS15",
                CustomersUsedBy = new List<Customer>(),
                CustomersValidFor = new List<Customer>(),
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                UseLimit = 5,
                Buy = 3,
                PayFor = 2,
                Products = new List<Product>
                {
                    _validProduct
                }
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
                        Amount = 1.5m,
                        Product = _validProduct
                    },
                    new Row
                    {
                        ProductPrice = 500,
                        Amount = 2.75m,
                        Product = Testdata.RandomProduct()
                    },
                    new Row
                    {
                        ProductPrice = 50,
                        Amount = 1.5m,
                        Product = _validProduct
                    }
                }
            };
        }

        /// <summary>
        ///     The discount should apply to the cheapest product in the cart
        /// </summary>
        [TestMethod]
        public void TestThatTheDiscountIsCalculatedOnTheChepestProduct()
        {
            _coupon.CalculateDiscount(_cart).should_be(50);
        }

        /// <summary>
        ///     If the discount applies for multiple products and thus spans more rows than
        ///     one the secound cheapest product shall be given the discount.
        /// </summary>
        [TestMethod]
        public void TestThatTheDiscountIsCalculatedOnThenNextRowIfTheFirstGotFree()
        {
            // ReSharper disable once PossibleNullReferenceException
            (_coupon as BuyXProductsPayForYProducts).PayFor = 0.5m;
            _coupon.CalculateDiscount(_cart).should_be(175);
        }

        /// <summary>
        ///     The discount can only ever apply to valid products
        /// </summary>
        [TestMethod]
        public void TestThatTheDiscountOnlyIsCalculatedOnValidProducts()
        {
            // ReSharper disable once PossibleNullReferenceException
            (_coupon as BuyXProductsPayForYProducts).Buy = 10;
            _coupon.CalculateDiscount(_cart).should_be(225);
        }
    }
}
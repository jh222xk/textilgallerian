using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using Domain.Tests.Helpers;
using NSpec;

namespace Domain.Tests.Entities
{
    [TestClass]
    public class BuyProductXRecieveProductYTest
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
                        NumberOfProducts = 2,
                        Product = _validProduct
                    },
                    new Row
                    {
                        ProductPrice = 500,
                        NumberOfProducts = 1,
                        Product = Testdata.RandomProduct()
                    }
                }
            };
        }

        /// <summary>
        ///     Test so outdated coupon is not valid even if customer is in CustomersValidFor-list
        /// </summary>
        [TestMethod]
        public void TestNotEnoughProductsForDiscount()
        {
            // ReSharper disable once PossibleNullReferenceException
            (_coupon as BuyXProductsPayForYProducts).Buy = 10;

            _coupon.IsValidFor(_cart).should_be_false();
        }

    }
}

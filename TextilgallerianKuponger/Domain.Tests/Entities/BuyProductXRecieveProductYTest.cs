using System;
using System.Collections.Generic;
using System.Linq;
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

            _coupon = new BuyProductXRecieveProductY
            {
                CanBeCombined = false,
                Code = "XMAS15",
                CustomersUsedBy = new List<Customer>(),
                CustomersValidFor = new List<Customer>(),
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                UseLimit = 5,
                Buy = 3,
                FreeProduct = _freeProduct,
                Amount = 2,
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
        ///     Test so if customer haven't enough products for the discount
        /// </summary>
        [TestMethod]
        public void TestCustomerHasNotEnoughProductsForDiscount()
        {
            // ReSharper disable once PossibleNullReferenceException
            (_coupon as BuyProductXRecieveProductY).Buy = 10;

            _coupon.IsValidFor(_cart).should_be_false();
        }

        /// <summary>
        ///     Test so outdated coupon is not valid even if customer is in CustomersValidFor-list
        /// </summary>
        [TestMethod]
        public void TestCustomerHasEnoughProductsForDiscount()
        {
            _coupon.IsValidFor(_cart).should_be_true();
        }

        /// <summary>
        ///     After calling the CalculateDiscount method the free product should have been added
        ///     to the cart
        /// </summary>
        [TestMethod]
        public void TestThatAFreeProductIsAddedToTheCart()
        {
            _coupon.CalculateDiscount(_cart).should_be(0);
            _cart.Rows.Count.should_be(3);
            _cart.Rows.Last().ProductPrice.should_be(0);
            _cart.Rows.Last().Product.should_be(_freeProduct);
            _cart.Rows.Last().Amount.should_be(2);
        }
    }
}

using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec;

namespace Domain.Tests.Entities
{
    [TestClass]
    public class TotalSumAmountDiscountTest
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

            _coupon = new TotalSumAmountDiscount
            {
                CanBeCombined = false,
                Code = "XMAS15",
                CustomersUsedBy = new List<Customer>(),
                CustomersValidFor = new List<Customer>(),
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                UseLimit = 5,
                Amount = 10000
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
        ///     Test so if customer haven't enough totalsum for the discount
        /// </summary>
        [TestMethod]
        public void TestInvalidIfTotalSumAmountDiscountIsNotOver()
        {
            _coupon.IsValidFor(_cart).should_be_false();
        }

        /// <summary>
        ///     Test so if customer have exact totalsum for the discount
        /// </summary>
        [TestMethod]
        public void TestValidIfTotalSumAmountDiscountIsExact()
        {
            // ReSharper disable once PossibleNullReferenceException
            (_coupon as TotalSumAmountDiscount).Amount = 700;

            _coupon.IsValidFor(_cart).should_be_true();
        }


        /// <summary>
        ///     Test so if customer have way over the totalsum for the discount
        /// </summary>
        [TestMethod]
        public void TestValidIfTotalSumIsOverAmountDiscount()
        {
            // ReSharper disable once PossibleNullReferenceException
            (_coupon as TotalSumAmountDiscount).Amount = 1500;
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
                        ProductPrice = 1500,
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

            _coupon.IsValidFor(_cart).should_be_true();
        }

    }
}

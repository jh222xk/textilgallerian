using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec;

namespace Domain.Tests.Entities
{
    [TestClass]
    public class TotalSumPercentageDiscountTest
    {
        private Cart _cart;
        private TotalSumPercentageDiscount _coupon;
        private Product _validProduct;

        /// <summary>
        ///     Setup our test data
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            _validProduct = Testdata.RandomProduct();

            _coupon = Testdata.RandomCoupon(new TotalSumPercentageDiscount
            {
                Percentage = 0.3m
            });

            _cart = new Cart
            {
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
                },
                Discounts = new List<Coupon>()
            };
        }

        /// <summary>
        ///     The sum of the discount is in percantage of the whole cart value
        /// </summary>
        [TestMethod]
        public void TestThatTheCorrectDiscountIsCalculated()
        {
            _coupon.CalculateDiscount(_cart).should_be(210);
        }
    }
}

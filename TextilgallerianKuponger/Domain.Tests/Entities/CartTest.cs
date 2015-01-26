using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using System.Collections.Generic;
using NSpec;

namespace Domain.Tests.Entities
{
    [TestClass]
    public class CartTest
    {
        private Cart cartOne;
        /// <summary>
        /// Setup our test data
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            Product productOne = new Product
            {
                ProductId = "My-Test-Product",
                Name = "A wonderful product"
            };

            Product productTwo = new Product
            {
                ProductId = "My-Test-Product-2",
                Name = "A not so wonderful product"
            };

            var rowList = new List<Row> {
                new Row
                {
                    Price = 100,
                    NumberOfProducts = 4,
                    Product = productOne
                },
                new Row
                {
                    Price = 500,
                    NumberOfProducts = 1,
                    Product = productTwo
                }
            };


            cartOne = new Cart {
                Rows = rowList
            };
        }

        [TestMethod]
        public void TestCanGetTotalSum()
        {
            cartOne.TotalSum.should_be(900);
        }

        [TestMethod]
        public void TestCanGetNumberOfProducts()
        {
            cartOne.NumberOfProducts.should_be(5);
        }
    }
}

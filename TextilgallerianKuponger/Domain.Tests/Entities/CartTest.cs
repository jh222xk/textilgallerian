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
        private Cart cart;

        /// <summary>
        /// Setup our test data
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            cart = new Cart
            {
                Rows = new List<Row>
                {
                    new Row
                    {
                        ProductPrice = 100,
                        NumberOfProducts = 4,
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
        /// Test for checking if the calculation of the TotalSum is correct
        /// </summary>
        [TestMethod]
        public void TestCanGetTotalSum()
        {
            // Check the TotalSum calculation
            cart.TotalSum.should_be(900);

            // Check that we really got 2 Rows
            cart.Rows.Count.should_be(2);
        }

        /// <summary>
        /// Test for checking if the calculation of the NumberOfProducts is correct
        /// </summary>
        [TestMethod]
        public void TestCanGetNumberOfProducts()
        {
            // Check the NumberOfProducts calculation
            cart.NumberOfProducts.should_be(5);
        }
    }
}

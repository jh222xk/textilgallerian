using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using System.Collections.Generic;
using NSpec;

namespace Domain.Tests.Entities
{
    [TestClass]
    public class RowTest
    {
        private Row rowOne;
        /// <summary>
        /// Setup our test data
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            Product productOne = new Product
            {
                ProductId = "My-Faked-Product",
                Name = "A faked product"
            };

            rowOne = new Row
            {
                Price = 900,
                NumberOfProducts = 6,
                Product = productOne
            };
        }

        /// <summary>
        /// Test for checking if the calculation of the Amount is correct
        /// </summary>
        [TestMethod]
        public void TestCanGetAmount()
        {
            rowOne.NumberOfProducts.should_be(6);
            rowOne.Price.should_be(900);

            // Check that the Product properties are what they should be
            rowOne.Product.ProductId.should_be("My-Faked-Product");
            rowOne.Product.Name.should_be("A faked product");

            // Check the amount calculation
            rowOne.Amount.should_be(5400);
        }
    }
}

using System.Collections.Generic;
using Domain.Entities;
using Domain.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec;

namespace Domain.Tests.Entities
{
    [TestClass]
    public class CartTest
    {
        private Cart cart;

        /// <summary>
        ///     Setup our test data
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
                        Amount = 4,
                        Product = Testdata.RandomProduct()
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
        ///     Test for checking if the calculation of the TotalSum is correct
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
        ///     Test for checking if the calculation of the Amount is correct
        /// </summary>
        [TestMethod]
        public void TestCanGetNumberOfProducts()
        {
            // Check the Amount calculation
            cart.NumberOfProducts.should_be(5);
        }
    }
}

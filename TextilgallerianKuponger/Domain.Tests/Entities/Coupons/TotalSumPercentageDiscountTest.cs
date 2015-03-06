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
        private TotalSumPercentageDiscount _couponWithoutProducts;
        private TotalSumPercentageDiscount _couponWithProducts;
        private Product _validProduct;

        /// <summary>
        ///     Setup our test data
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            _validProduct = Testdata.RandomProduct();

            _couponWithoutProducts = Testdata.RandomCoupon(new TotalSumPercentageDiscount
            {
                Percentage = 0.3m
            });
            _couponWithoutProducts.Products = null;

            _couponWithProducts = new TotalSumPercentageDiscount()
            {
                Percentage = 0.1m,
                Products = new List<Product>
                {
                    _validProduct
                },
                DiscountOnlyOnSpecifiedProducts = true,
            };

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
        ///     The sum of the discount is in percantage of the whole cart value. The coupon has no specified products.
        /// </summary>
        [TestMethod]
        public void TestThatTheCorrectDiscountIsCalculatedIfNoProductsSpecified()
        {
            _couponWithoutProducts.CalculateDiscount(_cart).should_be(210);
        }

        /// <summary>
        /// Check that discount is correct if not all products are valid for the discount
        /// </summary>
        [TestMethod]
        public void TestThatDiscountIsCorrectIfProductsAreSpecifiedAndValid()
        {
            _couponWithProducts.CalculateDiscount(_cart).should_be(20);
        }

        /// <summary>
        /// Check that discount is correct if not all products are valid for the discount
        /// </summary>
        [TestMethod]
        public void TestThatDiscountIsCorrectIfAllProductsShouldBeDiscounted()
        {
            _couponWithProducts.DiscountOnlyOnSpecifiedProducts = false;
            _couponWithProducts.CalculateDiscount(_cart).should_be(70);
        }

        /// <summary>
        /// Check so discount is 0 if the products specified in the coupon is not in the provided cart.
        /// </summary>
        [TestMethod]
        public void TestThatDiscountIsZeroIfProductsAreSpecifiedButNotInCart()
        {
            _couponWithProducts.Products = new List<Product>
            {
                new Product()
                {
                    ProductId = "notTheOneInCart"
                }
            };
            _couponWithProducts.CalculateDiscount(_cart).should_be(0);
        }


    }
}
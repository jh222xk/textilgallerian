using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Linq;
namespace Domain.Entities
{
    /// <summary>
    ///     Discount: Customer gets product Y for free when buying product(s) X
    /// </summary>
    public class BuyProductXRecieveProductY : Coupon
    {
        public BuyProductXRecieveProductY(IReadOnlyDictionary<string, string> properties) : base(properties)
        {
            SetValues(properties);
        }

        public override void SetValues(IReadOnlyDictionary<string, string> properties)
        {
            base.SetValues(properties);
            Amount = Decimal.Parse(properties["Amount"]);
            Buy = Decimal.Parse(properties["Buy"]);
        }

        public BuyProductXRecieveProductY()
        {
        }

        public override Dictionary<string, string> EditCoupon()
        {
            Dictionary<string, string> dictionary = base.EditCoupon();
            dictionary.Add("Amount", Amount.ToString());
            dictionary.Add("Buy", Buy.ToString());
            dictionary.Add("FreeProduct", FreeProduct.ProductId.ToString());
            dictionary.Add("ProductName", FreeProduct.Name.ToString());
            return dictionary;
        }

        /// <summary>
        ///     A free product we can get
        /// </summary>
        public Product FreeProduct { get; set; }

        /// <summary>
        ///     How many free products
        /// </summary>
        public Decimal Amount { get; set; }

        ///// <summary>
        /////     How many products customer need to buy
        ///// </summary>
        public Decimal Buy { get; set; }

        /// <summary>
        ///     Returns the dicount in amount of money, this method may have side effects like adding a free product to the cart
        ///     and shuld therfore only evere be called once per coupon if it's actually valid.
        /// </summary>
        public override Decimal CalculateDiscount(Cart cart)
        {
            cart.Rows.Add(new Row
            {
                Amount = Amount,
                Product = FreeProduct,
                ProductPrice = 0
            });
            return 0; // This coupon gives a free product instead of a sum of money
        }

        /// <summary>

        /// <summary>
        /// Check all products in cart if they are valid for discount.
        /// </summary>
        /// <param name="cart"></param>
        public override Boolean IsValidFor(Cart cart)
        {
            if (base.IsValidFor(cart) == false)
            {
                return false;
            }

            var products = cart.Rows.Select(row => row.Product).ToList();
            return products.Exists(p => p.In(Products)) && cart.NumberOfProducts >= Buy;
        }
    }
}
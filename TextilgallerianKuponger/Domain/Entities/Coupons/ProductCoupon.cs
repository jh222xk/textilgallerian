using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Linq;

namespace Domain.Entities
{
    /// <summary>
    ///     Coupons for specified products
    /// </summary>
    public abstract class ProductCoupon : Coupon
    {
        protected ProductCoupon(IReadOnlyDictionary<string, string> properties) : base(properties)
        {
            Buy = Decimal.Parse(properties["Buy"]);
        }

        public ProductCoupon()
        {
        }

        /// <summary>
        ///     Products valid for this discount
        /// </summary>
        public List<Product> Products { get; set; }

        /// <summary>
        ///     How many products customer need to buy
        /// </summary>
        public Decimal Buy { get; set; }

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
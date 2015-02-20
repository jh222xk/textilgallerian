using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Linq;

namespace Domain.Entities
{
    /// <summary>
    ///     Discount: Customer only pays for Y Products when buying X products from Products-list
    /// </summary>
    public class BuyXProductsPayForYProducts : Coupon
    {
        public BuyXProductsPayForYProducts(IReadOnlyDictionary<string, string> properties) : base(properties)
        {
            PayFor = Decimal.Parse(properties["PayFor"]);
        }

        public BuyXProductsPayForYProducts()
        {
        }

        /// <summary>
        ///     How many free products
        /// </summary>
        public Decimal PayFor { get; set; }

        /// <summary>
        ///     Returns the dicount in amount of money, this method may have side effects like adding a free product to the cart
        ///     and shuld therfore only evere be called once per coupon if it's actually valid.
        /// </summary>
        public override Decimal CalculateDiscount(Cart cart)
        {
            var products =
                cart.Rows.Where(r => r.Product.In(Products)).OrderBy(r => r.ProductPrice).ToList();
            var free = Buy - PayFor;
            Decimal discount = 0;

            while (free > 0 && products.Count > 0)
            {
                var cheapestProduct = products.First();
                if (free > cheapestProduct.Amount)
                {
                    discount += cheapestProduct.Amount*cheapestProduct.ProductPrice;
                }
                else
                {
                    discount += free*cheapestProduct.ProductPrice;
                }
                free -= cheapestProduct.Amount;
                products.RemoveAt(0);
            }

            return discount;
        }

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
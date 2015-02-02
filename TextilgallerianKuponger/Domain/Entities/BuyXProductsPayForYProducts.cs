using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Linq;

namespace Domain.Entities
{
    /// <summary>
    /// Discount: Customer only pays for Y Products when buying X products from Products-list
    /// </summary>
    public class BuyXProductsPayForYProducts : ProductCoupon
    {
        /// <summary>
        /// How many free products
        /// </summary>
        public Decimal PayFor { get; set; }

        /// <summary>
        /// Check if specified Cart is valid for this Coupon
        /// </summary>
        public override bool IsValidFor(Cart cart)
        {
            if (base.IsValidFor(cart) == false)
            {
                return false;
            }

            return cart.NumberOfProducts >= Buy;
        }

        public override Decimal CalculateDiscount(Cart cart)
        {
            var products = cart.Rows.Where(r => r.Product.In(Products)).OrderBy(r => r.ProductPrice).ToList();
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
    }
}

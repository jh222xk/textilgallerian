using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Linq;

namespace Domain.Entities
{
    /// <summary>
    /// Coupons for specified products
    /// </summary>
    public abstract class ProductCoupon : Coupon
    {
        /// <summary>
        /// Products valid for this discount
        /// </summary>
        public List<Product> Products { get; set; }

        public override bool IsValidFor(Cart cart)
        {
            if (base.IsValidFor(cart) == false)
            {
                return false;
            }

            var product = cart.Rows.Select(prod => prod.Product).ToList();

            return product.Exists(prod => prod.In(Products));
        }
    }
}

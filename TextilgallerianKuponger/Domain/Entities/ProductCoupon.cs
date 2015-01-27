using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

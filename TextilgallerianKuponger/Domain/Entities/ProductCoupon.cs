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
        //Products valid for this discount
        public List<Product> Products { get; set; }
    }
}

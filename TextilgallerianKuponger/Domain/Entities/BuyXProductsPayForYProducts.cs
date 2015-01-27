using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Discount: Customer only pays for Y Products when buying X products from Products-list
    /// </summary>
    public class BuyXProductsPayForYProducts : ProductCoupon
    {

        /// <summary>
        /// How many products customer need to buy
        /// </summary>
        public int Buy { get; set; }

        /// <summary>
        /// How many free products
        /// </summary>
        public int PayFor { get; set; }

        /// <summary>
        /// Check if specified Cart is valid for this Coupon
        /// </summary>
        public override bool IsValidFor(Cart cart)
        {
            throw new NotImplementedException();
        }
    }
}

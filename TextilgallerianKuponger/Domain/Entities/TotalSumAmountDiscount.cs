using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Class to give discount in kronor from total sum.
    /// </summary>
    public class TotalSumAmountDiscount : Coupon
    {
        public Decimal Amount { get; set; }

        /// <summary>
        /// Check if specified Cart is valid for this Coupon
        /// </summary>
        public override bool IsValidFor(Cart cart)
        {
            return cart.TotalSum >= Amount;
        }
    }
}

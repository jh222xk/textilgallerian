using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Class to give percentage discount from total sum.
    /// </summary>
    public class TotalSumPercentageDiscount : Coupon
    {
        public Decimal Percentage { get; set; }

        /// <summary>
        /// Check if specified Cart is valid for this Coupon
        /// </summary>
        public override bool IsValidFor(Cart cart)
        {
            throw new NotImplementedException();
        }
    }
}

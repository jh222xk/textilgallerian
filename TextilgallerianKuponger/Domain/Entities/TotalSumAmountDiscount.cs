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

        public override Cart IsValidFor(Cart cart)
        {
            throw new NotImplementedException();
        }
    }
}

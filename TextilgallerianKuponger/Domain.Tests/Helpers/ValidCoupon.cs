using System;
using Domain.Entities;

namespace Domain.Tests.Helpers
{
    /// <summary>
    ///     A coupon that is allways valid
    /// </summary>
    public class ValidCoupon : Coupon
    {
        public override Boolean IsValidFor(Cart cart)
        {
            return true;
        }

        public override Decimal CalculateDiscount(Cart cart)
        {
            return 0;
        }
    }
}

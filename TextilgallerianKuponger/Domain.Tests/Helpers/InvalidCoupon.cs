using System;
using Domain.Entities;

namespace Domain.Tests.Helpers
{
    /// <summary>
    ///     A coupon that is never valid
    /// </summary>
    public class InvalidCoupon : Coupon
    {
        public override Boolean IsValidFor(Cart cart)
        {
            return false;
        }

        public override Decimal CalculateDiscount(Cart cart)
        {
            return 0;
        }
    }
}
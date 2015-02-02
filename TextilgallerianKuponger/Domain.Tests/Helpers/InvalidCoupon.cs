using Domain.Entities;

namespace Domain.Tests.Helpers
{
    /// <summary>
    ///     A coupon that is never valid
    /// </summary>
    internal class InvalidCoupon : Coupon
    {
        public override bool IsValidFor(Cart cart)
        {
            return false;
        }
    }
}

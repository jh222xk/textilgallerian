using Domain.Entities;

namespace Domain.Tests.Helpers
{
    /// <summary>
    ///     A coupon that is allways valid
    /// </summary>
    internal class ValidCoupon : Coupon
    {
        public override bool IsValidFor(Cart cart)
        {
            return true;
        }
    }
}

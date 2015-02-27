using System;

namespace Domain.Entities
{
    /// <summary>
    ///     Customer to check if valid for discount
    /// </summary>
    public class Customer
    {
        public String SocialSecurityNumber { get; set; }
        public String Email { get; set; }
        public String CouponCode { get; set; }
        //number of times the customer has used a coupon.
        public int CouponUses { get; set; }
    }
}
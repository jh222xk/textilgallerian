using System.Collections.Generic;
using Domain.Entities;

namespace AdminView.ViewModel
{
    public class CouponViewModel
    {
        public int ClickCount { get; set; }
        public Coupon Coupon { get; set; }
        public IEnumerable<User> Users { get; set; }
        public User User { get; set; }

        public IEnumerable<Coupon> Coupons { get; set; }
    }
}
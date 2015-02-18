using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;

namespace AdminView.ViewModel
{
    public class PagedCouponsViewModel
    {
        public IEnumerable<Coupon> Coupons { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
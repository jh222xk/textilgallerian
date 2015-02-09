using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Domain.Entities;
using Domain.Entities.User;

namespace AdminView.ViewModel
{
    public class CouponViewModel
    {

        public int ClickCount { get; set; }
        public IEnumerable<Coupon> Coupons { get; set; }
        public Coupon Coupon { get; set; }
        public IEnumerable<User> Users { get; set;}
        public User User { get; set; }
        


    }
}
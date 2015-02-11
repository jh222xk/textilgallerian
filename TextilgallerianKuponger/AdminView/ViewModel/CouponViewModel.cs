using System.Collections.Generic;
using Domain.Entities;
using System;

namespace AdminView.ViewModel
{
    public class CouponViewModel
    {
        public Dictionary<string, String> CouponTypes = new Dictionary<string, String>() 
        {
            {  "Tag X betala för Y", typeof ( BuyXProductsPayForYProducts).Name },
            {  "Köp X få Y gratis", typeof (BuyProductXRecieveProductY).Name },
            {  "Köp för X:kr betala Y:kr", typeof ( TotalSumAmountDiscount).Name },
            {  "Köp för X:kr få Y:% rabatt", typeof ( TotalSumPercentageDiscount).Name }
        };


        public BuyXProductsPayForYProducts BuyXPayForY { get; set; }
        public int ClickCount { get; set; }
        public Coupon Coupon { get; set; }
        public IEnumerable<User> Users { get; set; }
        public User User { get; set; }

        public IEnumerable<Coupon> Coupons { get; set; }
    }
}
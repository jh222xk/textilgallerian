using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;

namespace AdminView.ViewModel
{
    public class CouponViewModel
    {
        public static Dictionary<String, String> CouponTypes = new Dictionary<String, String>
        {
            {typeof (BuyProductXRecieveProductY).FullName, "Köp X få Y gratis"},
            {typeof (BuyXProductsPayForYProducts).FullName, "Tag X betala för Y"},
            {typeof (TotalSumAmountDiscount).FullName, "Köp för X:kr få Y:kr rabatt"},
            {typeof (TotalSumPercentageDiscount).FullName, "Köp för X:kr få Y:% rabatt"}
        };

        public String Type { get; set; }
        public Boolean CanBeCombined { get; set; }
        public Dictionary<String, String> Parameters { get; set; }
    }
}
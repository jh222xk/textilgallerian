using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;

namespace AdminView.ExtensionMethods
{
    public static class TypeExtension
    {
        public static String Type(this Coupon coupon)
        {
            return Types[coupon.GetType().FullName];
        }

        public static readonly Dictionary<String, String> Types =
            new Dictionary<String, String>
            {
                {typeof (BuyProductXRecieveProductY).FullName, "Köp X få Y gratis"},
                {typeof (BuyXProductsPayForYProducts).FullName, "Tag X betala för Y"},
                {typeof (TotalSumAmountDiscount).FullName, "Köp för X:kr få Y:kr rabatt"},
                {typeof (TotalSumPercentageDiscount).FullName, "Köp för X:kr få Y:% rabatt"}
            };
    }
}
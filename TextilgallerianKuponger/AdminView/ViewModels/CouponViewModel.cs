using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using System.Net.Mail;
using System.Text.RegularExpressions;

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


        public List<Customer> Customers { get; set; }
        public string CustomerString { get; set; }
        public String Type { get; set; }
        public Boolean CanBeCombined { get; set; }
        public Dictionary<String, String> Parameters { get; set; }
    }
}
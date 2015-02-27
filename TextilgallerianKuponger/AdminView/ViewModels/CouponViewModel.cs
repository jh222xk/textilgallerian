using System;
using System.Collections.Generic;
using AdminView.ExtensionMethods;
using Domain.Entities;

namespace AdminView.ViewModel
{
    public class CouponViewModel
    {
        public static Dictionary<String, String> CouponTypes = TypeExtension.Types;


        public List<Product> Products { get; set; }
        public string ProductsString { get; set; }
        public List<Customer> Customers { get; set; }
        public string CustomerString { get; set; }
        public String Type { get; set; }
        public Boolean CanBeCombined { get; set; }
        public Dictionary<String, String> Parameters { get; set; }
        public Boolean DisposableCodes { get; set; }
        public int NumberOfCodes { get; set; }
    }
}
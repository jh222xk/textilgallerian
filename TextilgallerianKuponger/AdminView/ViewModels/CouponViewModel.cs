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


        public List<Customer> Customers()
        {
            List<Customer> customers = new List<Customer>();

            string[] lines = CustomerString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);


            foreach (string line in lines)
            {
                Customer customer = new Customer();
                customer.CouponUses = 0;

                Regex mailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Regex ssnRegex = new Regex(@"^[0-9]{6,8}-?[0-9]{4}$");

                //email customers
                if (mailRegex.Match(line).Success)
                {
                    customer.Email = line;
                    customers.Add(customer);
                }

                //ssn customers
                else if (ssnRegex.Match(line).Success)
                {
                    customer.SocialSecurityNumber = line;
                    customers.Add(customer);
                }
            }

            //if no customers are given, the coupon will be valid for everyone.
            return customers.Count > 0 ? customers : null;
        }

        public string CustomerString { get; set; }

        public String Type { get; set; }
        public Boolean CanBeCombined { get; set; }
        public Dictionary<String, String> Parameters { get; set; }
    }
}
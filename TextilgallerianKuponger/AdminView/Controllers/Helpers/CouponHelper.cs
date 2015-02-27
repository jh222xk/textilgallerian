using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AdminView.Controllers.Helpers
{
    public class CouponHelper
    {
        private readonly Random _random = new Random();
        private const string Chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public List<Customer> GetCustomers(string[] lines)
        {
            
            var customers = new List<Customer>();
           
            var mailRegex = new Regex(@"^.+?@.+?\.\w{2,8}$");
            var ssnRegex = new Regex(@"^[0-9]{6,8}-?[0-9]{4}$");

            foreach (var line in lines)
            {
                var customer = new Customer { CouponUses = 0 };

                // Match email
                if (mailRegex.Match(line).Success)
                {
                    customer.Email = line;
                    customers.Add(customer);
                }

                // Match social security number
                else if (ssnRegex.Match(line).Success)
                {
                    customer.SocialSecurityNumber = line;
                    customers.Add(customer);
                }
            }

            return customers.Count > 0 ? customers : null;
        }

        public List<Product> GetProducts(string[] lines)
        {
            var products = new List<Product>();

            var productId = new Regex(@"^\d{8}$");

            foreach (var line in lines)
            {
                var product = new Product();

                if (productId.Match(line).Success)
                {
                    product.ProductId = line;
                    products.Add(product);
                }
            }
            return products.Count > 0 ? products : null;
        }

        public List<Customer> GenerateDispoableCodes(int amount)
        {
            var customers = new List<Customer>(amount);

            for (var i = 0; i < amount; i++)
            {
                customers.Add(new Customer
                {
                    CouponCode = RandomString(8)
                });
            }

            return customers;
        }

        private string RandomString(int size)
        {
            var buffer = new char[size];

            for (var i = 0; i < size; i++)
            {
                buffer[i] = Chars[_random.Next(Chars.Length)];
            }

            return new string(buffer);
        }
    }
}
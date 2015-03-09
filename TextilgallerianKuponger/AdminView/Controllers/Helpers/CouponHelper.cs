using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdminView.Controllers.Helpers
{
    public class CouponHelper
    {
        private readonly Random _random = new Random();
        private const string Chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        /// <summary>
        /// Creates a list of customers from textarea input.
        /// </summary>
        /// <param name="customerString">A string from a textarea, where each row is a new customer</param>
        /// <returns>The list of customers</returns>
        public List<Customer> GetCustomers(string customerString)
        {
            //return null if no input
            if (customerString == null) { return null; }

            //split at newline and create array.
            var lines = customerString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
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

            //null if no customers.
            return customers.Count > 0 ? customers : null;
        }


        /// <summary>
        /// Creates a list of brands from textarea input.
        /// </summary>
        /// <param name="brandInput">A string from a textarea, where each row is a new brand</param>
        /// <returns>The list of brands</returns>
        public List<Brand> GetBrands(string brandInput)
        {
            //return null if no input
            if (brandInput == null) { return null; }

            //split at newline and create array.
            string[] lines = brandInput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var brands = new List<Brand>();

            foreach(var line in lines)
            {
                var brand = new Brand{BrandName = line};

                brands.Add(brand);
            }

            //null if no customers.
            return brands.Count > 0 ? brands : null;
        }


        /// <summary>
        /// Creates a list of categories from textarea input.
        /// </summary>
        /// <param name="categoryInput">A string from a textarea, where each row is a new category</param>
        /// <returns>The list of cateogires</returns>
        public List<Category> GetCategories(string categoryInput)
        {
            //return null if no input
            if (categoryInput == null) { return null; }

            //split at newline and create array.
            string[] lines = categoryInput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var categories = new List<Category>();

            foreach (var line in lines)
            {
                var category = new Category { CategoryName = line };

                categories.Add(category);
            }

            //null if no customers.
            return categories.Count > 0 ? categories : null;
        }

        /// <summary>
        /// Creates a list of products from textarea input.
        /// </summary>
        /// <param name="productsString">A string from a textarea, where each row is a new product</param>
        /// <returns>The list of products</returns>
        public List<Product> GetProducts(string productsString)
        {
            //return null if no input
            if (productsString == null) { return null; }

            //split at newline and create array.
            var lines = productsString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var products = new List<Product>();

            //Loop through all lines, and split on : if it exist, add the value to right of it, if no : just add line to productID.
            foreach (var line in lines)
            {
                var product = new Product();
                var splitLine = line.Split(':');

                switch (splitLine.Length)
                {
                    case 2:
                        product.Name = splitLine[0];
                        product.ProductId = splitLine[1];
                        break;
                    case 1:
                        product.ProductId = splitLine[0];
                        break;
                }   
               
            }
            return products.Count > 0 ? products : null;
        }

        /// <summary>
        /// Creates disposable coupons for many customers at once. 
        /// </summary>
        /// <param name="amount">Number of customers.</param>
        /// <returns></returns>
        public List<Customer> GenerateDisposableCodes(int amount)
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

        /// <summary>
        /// Creates a random string that can be used as a coupon code
        /// </summary>
        /// <param name="size">string length</param>
        /// <returns></returns>
        public string RandomString(int size)
        {
            var buffer = new char[size];

            for (var i = 0; i < size; i++)
            {
                buffer[i] = Chars[_random.Next(Chars.Length)];
            }

            return new string(buffer);
        }

        /// <summary>
        /// Creates a string for a textarea from a list of customers
        /// </summary>
        /// <param name="customers">List of customers</param>
        /// <returns></returns>
        public string CreateCustomerString(List<Customer> customers)
        {
            //Uses email if available. If not, social security number.
            return string.Join(Environment.NewLine, customers.Select(c => c.Email != null ? c.Email : c.SocialSecurityNumber));
        }

        /// <summary>
        /// Creates a string for a textarea from a list of products
        /// </summary>
        /// <param name="products">list of products</param>
        /// <returns></returns>
        public string CreateProductsString(List<Product> products)
        {
            return string.Join(Environment.NewLine, products.Select(p => p.ProductId));
        }

        /// <summary>
        /// Creates a string for a textarea from a list of brands
        /// </summary>
        /// <param name="brands">List of brands</param>
        /// <returns></returns>
        public string CreateBrandString(List<Brand> brands)
        {
            return string.Join(Environment.NewLine, brands.Select(b => b.BrandName));
        }

        /// <summary>
        /// Creates a string for a textarea from a list of categories
        /// </summary>
        /// <param name="categories">list of categories</param>
        /// <returns></returns>
        public string CreateCategoryString(List<Category> categories)
        {
            return string.Join(Environment.NewLine, categories.Select(c => c.CategoryName));
        }

        /// <summary>
        /// Creates a coupon from a type and a list of strings describing the properties and their values.
        /// </summary>
        /// <param name="typeName">A string with the name of the coupon type.</param>
        /// <param name="parameters">A dictionary for the coupon's properties and their values</param>
        /// <returns></returns>
        public Coupon CreateCoupon(string typeName, Dictionary<string, string> parameters)
        {
            //gets type of Coupon
            var type = Assembly.GetAssembly(typeof(Coupon)).GetType(typeName);

            // Magic super perfect code, do not touch!
            //Gets the constructor for this type and runs it to create coupon.
            var constructor = type.GetConstructor(new[] { typeof(IReadOnlyDictionary<String, String>) });
            var coupon = constructor.Invoke(new object[] { parameters }) as Coupon;

            return coupon;
        }
    }
}
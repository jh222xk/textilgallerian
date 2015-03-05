using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Linq;
using System.Globalization;

namespace Domain.Entities
{
    /// <summary>
    ///     Discount: Customer only pays for Y Products when buying X products from Products-list
    /// </summary>
    public class BuyXProductsPayForYProducts : Coupon
    {
        public BuyXProductsPayForYProducts(IReadOnlyDictionary<string, string> properties)
        {
            SetProperties(properties);
        }

        public override void SetProperties(IReadOnlyDictionary<string, string> properties)
        {
            base.SetProperties(properties);
            PayFor = Decimal.Parse(properties["PayFor"], CultureInfo.InvariantCulture);
        }

        public override Dictionary<string, string> GetProperties() 
        {
            var dictionary = base.GetProperties();
            dictionary.Add("PayFor", PayFor.ToString(CultureInfo.InvariantCulture));

            return dictionary;
        }

        public BuyXProductsPayForYProducts()
        {
        }

        /// <summary>
        ///     How many free products
        /// </summary>
        public Decimal PayFor { get; set; }

        /// <summary>
        ///     Returns the dicount in amount of money, this method may have side effects like adding a free product to the cart
        ///     and shuld therfore only evere be called once per coupon if it's actually valid.
        /// </summary>
        public override Decimal CalculateDiscount(Cart cart)
        {
            var products =
                cart.Rows.Where(r => r.Product.In(Products)).OrderBy(r => r.ProductPrice).ToList();
            var free = NumberOfProductsToBuy - PayFor;
            Decimal discount = 0;

            while (free > 0 && products.Count > 0)
            {
                var cheapestProduct = products.First();
                if (free > cheapestProduct.Amount)
                {
                    discount += cheapestProduct.Amount*cheapestProduct.ProductPrice;
                }
                else
                {
                    discount += free*cheapestProduct.ProductPrice;
                }
                free -= cheapestProduct.Amount;
                products.RemoveAt(0);
            }

            return discount;
        }
    }
}
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
        public BuyXProductsPayForYProducts(IReadOnlyDictionary<String, String> properties)
        {
            SetProperties(properties);
        }

        public override void SetProperties(IReadOnlyDictionary<String, String> properties)
        {
            base.SetProperties(properties);
            PayFor = Decimal.Parse(properties["PayFor"], CultureInfo.InvariantCulture);
        }

        public override Dictionary<String, String> GetProperties() 
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
            var rows = cart.Rows;
            Decimal discount = 0;

            if (Products != null)
            {
                rows = rows.Where(row => Products.Exists(p => p.ProductId == row.Product.ProductId)).ToList();
            }

            if (Brands != null)
            {
                rows = rows.Where(row => Brands.Exists(b => b.BrandName == row.Brand.BrandName)).ToList();
            }

            if (Categories != null)
            {
                rows =
                    rows.Where(
                        row => Categories.Exists(c => row.Categories.Select(e => e.CategoryName).Contains(c.CategoryName)))
                        .ToList();
            }

            var free = NumberOfProductsToBuy - PayFor;

            // Order by cheapest first
            rows = rows.OrderBy(r => r.ProductPrice).ToList();

            while (free > 0 && rows.Count > 0)
            {
                var cheapestProduct = rows.First();
                if (free > cheapestProduct.Amount)
                {
                    discount += cheapestProduct.Amount*cheapestProduct.ProductPrice;
                }
                else
                {
                    discount += free*cheapestProduct.ProductPrice;
                }
                free -= cheapestProduct.Amount;
                rows.RemoveAt(0);
            }

            return discount;
        }
    }
}
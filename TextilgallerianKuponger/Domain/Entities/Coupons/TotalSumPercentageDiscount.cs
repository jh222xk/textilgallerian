using System;
using System.Collections.Generic;
using System.Globalization;

namespace Domain.Entities
{
    /// <summary>
    ///     Class to give percentage discount from total sum.
    /// </summary>
    public class TotalSumPercentageDiscount : Coupon
    {
        public TotalSumPercentageDiscount(IReadOnlyDictionary<string, string> properties)
        {
            SetProperties(properties);
        }

        public TotalSumPercentageDiscount()
        {
        }

        public override void SetProperties(IReadOnlyDictionary<string, string> properties)
        {
            base.SetProperties(properties);
            Percentage = Decimal.Parse(properties["Percentage"], CultureInfo.InvariantCulture);
        }

        public override Dictionary<string, string> GetProperties()
        {
            var dictionary = base.GetProperties();
            dictionary.Add("Percentage", Percentage.ToString(CultureInfo.InvariantCulture));

            return dictionary;
        }

        public Decimal Percentage { get; set; }

        /// <summary>
        ///     Returns the dicount in amount of money, this method may have side effects like adding a free product to the cart
        ///     and shuld therfore only evere be called once per coupon if it's actually valid.
        /// </summary>
        public override Decimal CalculateDiscount(Cart cart)
        {
            Decimal discount = 0;

            if(Products == null)
            {
                return cart.TotalSum * Percentage;
            }
            else
            {
                foreach (Row row in cart.Rows)
                {
                    if(Products.Exists(p => p.ProductId == row.Product.ProductId))
                    {
                        discount = row.ProductPrice * Percentage;
                    }
                }
            }
            return discount;
        }
    }
}
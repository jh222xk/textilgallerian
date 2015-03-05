using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Domain.Entities
{
    /// <summary>
    ///     Class to give percentage discount from total sum.
    /// </summary>
    public class TotalSumPercentageDiscount : Coupon
    {
        public TotalSumPercentageDiscount(IReadOnlyDictionary<String, String> properties)
        {
            SetProperties(properties);
        }

        public TotalSumPercentageDiscount()
        {
        }

        /// <summary>
        ///     If set to true the percentage will only be calculated on Products that is valid
        ///     for this Coupon, if set to false the percentage will be calculated on all products
        ///     in the cart if any of the vaild products is present.
        /// </summary>
        public Boolean DiscountOnlyOnSpecifiedProducts { get; set; }

        /// <summary>
        ///     A Percentage between 0 and 1
        /// </summary>
        public Decimal Percentage { get; set; }

        public override void SetProperties(IReadOnlyDictionary<String, String> properties)
        {
            base.SetProperties(properties);
            Percentage = Decimal.Parse(properties["Percentage"], CultureInfo.InvariantCulture) / 100;
            if (Percentage < 0 || Percentage > 1)
            {
                throw new ArgumentException("Percentage must be between 0 and 100");
            }
        }

        public override Dictionary<String, String> GetProperties()
        {
            var dictionary = base.GetProperties();
            dictionary.Add("Percentage", (Percentage * 100).ToString(CultureInfo.InvariantCulture));

            return dictionary;
        }

        /// <summary>
        ///     Returns the dicount in amount of money, this method may have side effects like adding a free product to the cart
        ///     and shuld therfore only evere be called once per coupon if it's actually valid.
        /// </summary>
        public override Decimal CalculateDiscount(Cart cart)
        {
            if (!DiscountOnlyOnSpecifiedProducts || Products == null)
            {
                return cart.TotalSum * Percentage;
            }

            // Only calulate the discount on products that are valid for this cupon
            return cart.Rows
                .Where(row => Products.Exists(p => p.ProductId == row.Product.ProductId))
                .Sum(row => row.TotalPrice * Percentage);
        }
    }
}
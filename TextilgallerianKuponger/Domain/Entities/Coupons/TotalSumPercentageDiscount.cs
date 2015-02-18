using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    ///     Class to give percentage discount from total sum.
    /// </summary>
    public class TotalSumPercentageDiscount : Coupon
    {
        public TotalSumPercentageDiscount(IReadOnlyDictionary<string, string> properties) : base(properties)
        {
            Percentage = Decimal.Parse(properties["Percentage"]);
        }

        public TotalSumPercentageDiscount()
        {
        }

        public Decimal Percentage { get; set; }

        /// <summary>
        ///     Returns the dicount in amount of money, this method may have side effects like adding a free product to the cart
        ///     and shuld therfore only evere be called once per coupon if it's actually valid.
        /// </summary>
        public override Decimal CalculateDiscount(Cart cart)
        {
            return cart.TotalSum*Percentage;
        }
    }
}
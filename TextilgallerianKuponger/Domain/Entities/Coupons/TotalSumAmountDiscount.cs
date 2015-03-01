using System;
using System.Collections.Generic;
using System.Globalization;

namespace Domain.Entities
{
    /// <summary>
    ///     Class to give discount in kronor from total sum.
    /// </summary>
    public class TotalSumAmountDiscount : Coupon
    {
        public TotalSumAmountDiscount(IReadOnlyDictionary<string, string> properties)
        {
            SetProperties(properties);
        }

        public TotalSumAmountDiscount()
        {
        }

        public override void SetProperties(IReadOnlyDictionary<string, string> properties)
        {
            base.SetProperties(properties);
            Amount = Decimal.Parse(properties["Amount"], CultureInfo.InvariantCulture);
        }

        public Decimal Amount { get; set; }

        public override Dictionary<string, string> GetProperties()
        {
            var dictionary = base.GetProperties();
            dictionary.Add("Amount", Amount.ToString(CultureInfo.InvariantCulture));

            return dictionary;
        }

        /// <summary>
        ///     Check if specified Cart is valid for this Coupon
        /// </summary>
        public override Boolean IsValidFor(Cart cart)
        {
            if (base.IsValidFor(cart) == false)
            {
                return false;
            }

            return cart.TotalSum >= Amount;
        }

        /// <summary>
        ///     Returns the dicount in amount of money, this method may have side effects like adding a free product to the cart
        ///     and shuld therfore only evere be called once per coupon if it's actually valid.
        /// </summary>
        public override Decimal CalculateDiscount(Cart cart)
        {
            return Amount;
        }
    }
}
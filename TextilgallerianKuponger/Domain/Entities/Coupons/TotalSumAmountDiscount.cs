using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    ///     Class to give discount in kronor from total sum.
    /// </summary>
    public class TotalSumAmountDiscount : Coupon
    {
        public TotalSumAmountDiscount(IReadOnlyDictionary<string, string> properties) : base(properties)
        {
            SetValues(properties);
        }

        public TotalSumAmountDiscount()
        {
        }

        public override void SetValues(IReadOnlyDictionary<string, string> properties)
        {
            base.SetValues(properties);
            Amount = Decimal.Parse(properties["Amount"]);
        }

        public Decimal Amount { get; set; }

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

        public override Dictionary<string, string> EditCoupon()
        {
            Dictionary<string, string> dictionary = base.EditCoupon();
            dictionary.Add("Amount", Amount.ToString());

            return dictionary;
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
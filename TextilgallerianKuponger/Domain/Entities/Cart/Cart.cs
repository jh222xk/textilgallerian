using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    /// <summary>
    /// Public Cart class
    /// </summary>
    public class Cart : ICloneable 
    {
        private Decimal? _discount;

        /// <summary>
        /// Code entered by the customer
        /// </summary>
        public String CouponCode { get; set; }

        /// <summary>
        /// The customer checking out this cart
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// A list with CartRows, which contains cart-items such as
        /// Price, Product, Amount and so on
        /// </summary>
        public List<Row> Rows { get; set; }

        /// <summary>
        /// A list of all discounts valid for this cart
        /// </summary>
        public List<Coupon> Discounts { get; set; }

        /// <summary>
        /// Total price for the entire cart with every product excluding discounts
        /// </summary>
        public Decimal TotalSum
        { 
            get
            {
                // TODO: Include shipping cost?
                return Rows.Sum(row => row.TotalPrice);
            }
        }

        /// <summary>
        /// Total discount with every coupon valid for this cart
        /// </summary>
        public Decimal CalculateDiscount()
        {
            if (!_discount.HasValue)
            {
                _discount = Discounts.Sum(d => d.CalculateDiscount(this));
            }
            return _discount.Value;
        }

        /// <summary>
        /// Total sum for the entire cart with every product with discount calculated
        /// </summary>
        public Decimal DiscountedSum
        {
            get
            {
                return TotalSum - CalculateDiscount();
            }
        }

        /// <summary>
        /// Total sum for the entire cart with every product with discount calculated
        /// </summary>
        public Decimal Discount
        {
            get
            {
                return CalculateDiscount();
            }
        }

        /// <summary>
        /// The number of products we have for the whole cart
        /// </summary>
        public Decimal NumberOfProducts
        {
            get
            {
                return Rows.Sum(row => row.Amount);
            }
        }

        /// <summary>
        ///     Creates a new object that is a copy of the current instance.
        /// </summary>
        public object Clone()
        {
            return new Cart
            {
                CouponCode = CouponCode,
                Customer = Customer,
                Rows = (Rows == null) ? new List<Row>() : new List<Row>(Rows),
                Discounts = (Discounts == null) ? new List<Coupon>() : new List<Coupon>(Discounts)
            };
        }
    }
}

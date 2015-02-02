using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Linq;

namespace Domain.Entities
{
    /// <summary>
    /// Discount: Customer gets product Y for free when buying product(s) X
    /// </summary>
    public class BuyProductXRecieveProductY : ProductCoupon
    {
        /// <summary>
        /// A free product we can get
        /// </summary>
        public Product FreeProduct { get; set; }

        /// <summary>
        /// How many free products
        /// </summary>
        public Decimal Amount { get; set; }

        public override Decimal CalculateDiscount(Cart cart)
        {
            cart.Rows.Add(new Row
            {
                Amount = Amount,
                Product = FreeProduct,
                ProductPrice = 0
            });
            return 0; // This coupon gives a free product instead of a sum of money
        }
    }
}

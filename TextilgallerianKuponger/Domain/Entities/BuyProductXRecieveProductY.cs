using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Discount: Customer gets product Y for free when buying product(s) X
    /// </summary>
    public class BuyProductXRecieveProductY : ProductCoupon
    {
        /// <summary>
        /// A (always one) free product we can get
        /// </summary>
        public Product FreeProduct { get; set; }

        public override Cart IsValidFor(Cart cart)
        {
            throw new NotImplementedException();
        }
    }
}

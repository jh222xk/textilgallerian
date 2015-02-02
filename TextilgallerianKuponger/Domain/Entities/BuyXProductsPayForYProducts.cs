using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Linq;

namespace Domain.Entities
{
    /// <summary>
    /// Discount: Customer only pays for Y Products when buying X products from Products-list
    /// </summary>
    public class BuyXProductsPayForYProducts : ProductCoupon
    {
        /// <summary>
        /// How many free products
        /// </summary>
        public Decimal PayFor { get; set; }
    }
}

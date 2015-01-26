using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Discount: Customer only pays for Y Products when buying X products from Products-list
    /// </summary>
    public class BuyXProductsPayForYProducts : ProductCoupon
    {

        // How many products customer need to buy
        public int Buy { get; set; }

        // How many free products
        public int PayFor { get; set; }

        public override Cart IsValidFor(Cart cart)
        {
            throw new NotImplementedException();
        }
    }
}

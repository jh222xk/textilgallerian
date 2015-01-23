using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Discount: Customer only pays for Y Products when buying X products from BuyProducts-list
    /// </summary>
    public class BuyXProductsPayForYProducts : Coupon
    {
        // List amount of Products needed to buy. 
        public List<Product> BuyProducts { get; set; }

        // How many customer need to buy
        public int Buy { get; set; }

        // How many free
        public int PayFor { get; set; }
    }
}

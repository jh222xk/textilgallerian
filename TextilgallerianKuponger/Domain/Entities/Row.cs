using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Describes a single Row in a Cart and contains cart-items such as 
    /// Price, Product, Amount and so on
    /// </summary>
    public class Row
    {
        /// <summary>
        /// Price for a single product for this row
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// The number of products for this row
        /// </summary>
        public int NumberOfProducts { get; set; }

        /// <summary>
        /// A single product for this row
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// The total price for this row (ProductPrice * NumberOfProducts)
        /// </summary>
        public decimal TotalPrice 
        {
            get
            {
                return NumberOfProducts * ProductPrice;
            }
        }
    }
}

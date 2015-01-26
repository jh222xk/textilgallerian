using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Public Row class that contains cart-items such as 
    /// Price, Product, Amount and so on
    /// </summary>
    public class Row
    {
        /// <summary>
        /// The price for our cart row
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The amount of products for this row
        /// </summary>
        public int NumberOfProducts { get; set; }

        /// <summary>
        /// A single product for this row
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Returns
        /// </summary>
        public decimal Amount 
        {
            get
            {
                return NumberOfProducts * Price;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Public Cart class
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// A list with CartRows, which contains cart-items such as
        /// Price, Product, Amount and so on
        /// </summary>
        public List<Row> Rows { get; set; }

        /// <summary>
        /// Total sum for the entire cart with every product
        /// </summary>
        public decimal TotalSum
        { 
            get
            {
                // TODO Inlude shipping cost?
                return Rows.Sum(row => row.TotalPrice);
            }
        }

        /// <summary>
        /// The number of products we have for the whole cart
        /// </summary>
        public int NumberOfProducts
        {
            get
            {
                return Rows.Sum(row => row.NumberOfProducts);
            }
        }
    }
}

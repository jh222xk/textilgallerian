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
        /// Total sum for the entire cart with every product
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
        /// The number of products we have for the whole cart
        /// </summary>
        public Decimal NumberOfProducts
        {
            get
            {
                return Rows.Sum(row => row.Amount);
            }
        }
    }
}

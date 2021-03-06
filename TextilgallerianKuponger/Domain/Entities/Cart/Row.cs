﻿using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    ///     Describes a single Row in a Cart and contains cart-items such as
    ///     Price, Product, Amount and so on
    /// </summary>
    public class Row
    {
        /// <summary>
        ///     Price for a single product for this row
        /// </summary>
        public Decimal ProductPrice { get; set; }

        /// <summary>
        ///     The number of products for this row
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        ///     A single product for this row
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        ///     
        /// </summary>
        public Brand Brand { get; set; }

        /// <summary>
        ///     A list of categories for the products on this row
        /// </summary>
        public List<Category> Categories { get; set; }

        /// <summary>
        ///     The total price for this row (ProductPrice * Amount)
        /// </summary>
        public Decimal TotalPrice
        {
            get { return Amount*ProductPrice; }
        }
    }
}
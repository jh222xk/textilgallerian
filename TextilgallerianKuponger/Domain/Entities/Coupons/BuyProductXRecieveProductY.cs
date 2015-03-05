using System;
using System.Collections.Generic;
using System.Globalization;

namespace Domain.Entities
{
    /// <summary>
    ///     Discount: Customer gets product Y for free when buying product(s) X
    /// </summary>
    public class BuyProductXRecieveProductY : Coupon
    {

        public BuyProductXRecieveProductY()
        {
        }

        //public BuyProductXRecieveProductY(IReadOnlyDictionary<string, string> properties) : base(properties)
        public BuyProductXRecieveProductY(IReadOnlyDictionary<string, string> properties)
        {
            SetProperties(properties);
        }

        public override void SetProperties(IReadOnlyDictionary<string, string> properties)
        {
            base.SetProperties(properties);
            AmountOfProducts = Decimal.Parse(properties["AmountOfProducts"], CultureInfo.InvariantCulture);
            //TODO: Check if "FreeProduct" is valid!!
            FreeProduct = new Product { ProductId = properties["FreeProduct"]};

        }

        public override Dictionary<string, string> GetProperties()
        {
            var dictionary = base.GetProperties();
            dictionary.Add("AmountOfProducts", AmountOfProducts.ToString(CultureInfo.InvariantCulture));
            dictionary.Add("FreeProduct", FreeProduct.ProductId);
            dictionary.Add("ProductName", FreeProduct.Name);

            return dictionary;
        }

        /// <summary>
        ///     A free product we can get
        /// </summary>
        public Product FreeProduct { get; set; }

        /// <summary>
        ///     How many free products
        /// </summary>
        public Decimal AmountOfProducts { get; set; }

        /// <summary>
        ///     Returns the dicount in amount of money, this method may have side effects like adding a free product to the cart
        ///     and shuld therfore only evere be called once per coupon if it's actually valid.
        /// </summary>
        public override Decimal CalculateDiscount(Cart cart)
        {
            cart.Rows.Add(new Row
            {
                Amount = AmountOfProducts,
                Product = FreeProduct,
                ProductPrice = 0
            });
            return 0; // This coupon gives a free product instead of a sum of money
        }
    }
}
using System;
using System.Collections.Generic;

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
            Amount = Decimal.Parse(properties["Amount"].Replace('.', ','));
            Buy = Decimal.Parse(properties["Buy"].Replace('.', ','));
            // Todo Add Freeproduct and productName ?
        }

    

        public override Dictionary<string, string> GetProperties()
        {
            var dictionary = base.GetProperties();
            dictionary.Add("Amount", Amount.ToString().Replace(',', '.'));
            dictionary.Add("Buy", Buy.ToString().Replace(',', '.'));
            dictionary.Add("FreeProduct", FreeProduct.ProductId.ToString());
            dictionary.Add("ProductName", FreeProduct.Name.ToString());

            return dictionary;
        }

        /// <summary>
        ///     A free product we can get
        /// </summary>
        public Product FreeProduct { get; set; }

        /// <summary>
        ///     How many free products
        /// </summary>
        public Decimal Amount { get; set; }

        ///// <summary>
        /////     How many products customer need to buy
        ///// </summary>
        public Decimal Buy { get; set; }

        /// <summary>
        ///     Returns the dicount in amount of money, this method may have side effects like adding a free product to the cart
        ///     and shuld therfore only evere be called once per coupon if it's actually valid.
        /// </summary>
        public override Decimal CalculateDiscount(Cart cart)
        {
            cart.Rows.Add(new Row
            {
                Amount = Amount,
                Product = FreeProduct,
                ProductPrice = 0
            });
            return 0; // This coupon gives a free product instead of a sum of money
        }

        /// <summary>
        ///     Check if specified Cart is valid for this Coupon
        /// </summary>
        public override bool IsValidFor(Cart cart)
        {
            if (!base.IsValidFor(cart))
            {
                return false;
            }

            return cart.NumberOfProducts >= Buy;
        }
    }
}
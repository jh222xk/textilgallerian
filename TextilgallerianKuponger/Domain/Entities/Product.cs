namespace Domain.Entities
{
    /// <summary>
    /// Products that may have discounts
    /// </summary>
    public class Product {
        public string ProductId { get; set; }
        public string Name { get; set; }

        /* We are unsure if Customer has catory and Brand when we cummunicate with purschase checkout. If it Has it is probably
        Better to use Category and Brand, if not, we will have to save a full list of product Id to our coupons. 

        public string Category { get; set; }
        public string Brand { get; set; }
         
         */
    }
}

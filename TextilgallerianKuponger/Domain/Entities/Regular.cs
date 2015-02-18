namespace Domain.Entities
{
    /// <summary>
    ///     Class to handle % discount on total sum for regulars.
    /// </summary>
    internal class Regular : Customer
    {
        public decimal Percentage { get; set; }
    }
}
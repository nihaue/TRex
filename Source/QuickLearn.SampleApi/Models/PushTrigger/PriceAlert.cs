using TRex.Metadata;

namespace QuickLearn.SampleApi.Models
{
    public class PriceAlert
    {
        [Metadata("Symbol", "Symbol being monitored for price changes")]
        public string Symbol { get; set; }

        [Metadata("Price", "Current price of the symbol at the time of the alert firing")]
        public decimal Price { get; set; }
    }
}
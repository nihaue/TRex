using TRex.Metadata;

namespace QuickLearn.SampleApi.Models
{
    public class PriceAlertConfig
    {

        public PriceAlertConfig()
        {


        }

        [Metadata("Symbol", "Symbol to monitor for price changes")]
        public string Symbol { get; set; }

        [Metadata("Price Target", "Price threshhold past which the trigger should fire")]
        public decimal TargetPrice { get; set; }

        [Metadata("Higher Is Better?", "Value that indicates whether the trigger should fire when the price is under the threshhold (false), or above (true)")]
        public bool HigherIsBetter { get; set; }

        [CallbackUrl]
        public string CallbackUrl { get; set; }
    }
}
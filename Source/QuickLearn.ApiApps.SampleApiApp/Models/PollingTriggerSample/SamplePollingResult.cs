using System;
using TRex.Metadata;

namespace QuickLearn.ApiApps.SampleApiApp.Models
{
    public class SamplePollingResult
    {
        public SamplePollingResult(int diceRoll)
        {
            DiceRoll = diceRoll;
            TimeStamp = DateTime.UtcNow;
        }

        [Metadata("Dice Roll", "The number that was rolled to kick off the Logic App")]
        public int DiceRoll { get; set; }

        [Metadata(Visibility = VisibilityType.Advanced)]
        public DateTime TimeStamp { get; set; }
    }
}

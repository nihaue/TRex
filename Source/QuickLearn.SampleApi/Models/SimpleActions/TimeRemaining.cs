using System;
using TRex.Metadata;

namespace QuickLearn.SampleApi.Models
{
    public class TimeRemaining
    {

        public TimeRemaining()
        {

        }

        public static TimeRemaining FromTimeSpan(TimeSpan remaining)
        {
            return new TimeRemaining()
            {
                Days = remaining.Days,
                Hours = remaining.Hours,
                Minutes = remaining.Minutes,
                Seconds = remaining.Seconds
            };
        }

        [Metadata("Days", "Days until the target date")]
        public int Days { get; set; }

        [Metadata("Hours", "Hours until the target date")]
        public int Hours { get; set; }

        [Metadata("Minutes", "Minutes until the target date")]
        public int Minutes { get; set; }

        [Metadata("Seconds", "Seconds until the target date")]
        public int Seconds { get; set; }
    }
}
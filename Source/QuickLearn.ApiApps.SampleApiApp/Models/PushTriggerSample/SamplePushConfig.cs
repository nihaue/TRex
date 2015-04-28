using TRex.Metadata;

namespace QuickLearn.ApiApps.SampleApiApp.Models
{
    public class SamplePushConfig
    {
        [Metadata("Quiet Hour (UTC)", "This is the hour of the day that trigger should be skipped in UTC time (0-23)")]
        public int QuietHour { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRex.Metadata;

namespace QuickLearn.ApiApps.SampleApiApp.Models
{
    public class SamplePollingResult
    {
        public SamplePollingResult()
        {
            SampleBoolean = true;
            TimeStamp = DateTime.UtcNow;
        }

        [Metadata("Sample Boolean Value", "Just a sample value again")]
        public bool SampleBoolean { get; set; }


        [Metadata(Visibility = VisibilityType.Advanced)]
        public DateTime TimeStamp { get; set; }
    }
}

using System;
using TRex.Metadata;

namespace QuickLearn.ApiApps.SampleApiApp.Models
{
    public class SamplePushEvent
    {
        public SamplePushEvent()
        {
            SampleIntProperty = 42;
            SampleStringProperty = "Forty-Two";
            SampleComplexProperty = new ClassB();
            TimeStamp = DateTime.UtcNow;
        }
        
        [Metadata(Visibility = VisibilityType.Advanced)]
        public DateTime TimeStamp { get; set; }

        [Metadata("A Wild Int Appeared", "This is an Int")]
        public int SampleIntProperty { get; set; }

        [Metadata("Sample String Again", "Another sample string, there are lots of these")]
        public string SampleStringProperty { get; set; }

        [Metadata(Visibility = VisibilityType.Advanced,
            FriendlyName = "Sample Complex Property", Description = "Sample complex property -- instance of ClassB")]
        public ClassB SampleComplexProperty { get; set; }
    }

    public class ClassB
    {
        public ClassB()
        {
            SampleDecimalProperty = 42.00M;
        }

        [Metadata("Decimal Value", "This is a decimal value")]
        public decimal SampleDecimalProperty { get; set; }

    }

}

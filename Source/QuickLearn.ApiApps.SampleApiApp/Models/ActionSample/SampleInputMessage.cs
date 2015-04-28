using TRex.Metadata;

namespace QuickLearn.ApiApps.SampleApiApp.Models
{
    public class SampleInputMessage
    {

        [Metadata("String Property", "A happy string input value")]
        public string StringProperty { get; set; }

        [Metadata(Visibility = VisibilityType.Advanced, FriendlyName = "Advanced String Property")]
        public string AdvancedStringProperty { get; set; }
       
    }
}

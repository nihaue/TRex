using TRex.Metadata;

namespace TRex.TestApiApp.Models
{
    public class NotificationContentTestModel
    {
        public string SampleProperty { get; set; }
        
        [Metadata("Sample Int Property", "Contains a sample int property", VisibilityType.Advanced)]
        public int SampleIntProperty { get; set; }
    }
}
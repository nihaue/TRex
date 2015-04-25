using TRex.Metadata;

namespace QuickLearn.ApiApps.SampleApiApp.Models
{
    public class SampleOutputMessage
    {
        public SampleOutputMessage()
        {
            StringProperty = "Value here";
            IntProperty = 42;
            StringArrayProperty = new string[] { "Happy", "Dappy", "Values", "Here" };
        }

        [Metadata(FriendlyName = "String Property", Description = "This is a happy string.")]
        public string StringProperty { get; set; }

        [Metadata(Visibility = VisibilityTypes.Advanced, FriendlyName = "Int Property", Description = "This is a happy int.")]
        public int IntProperty { get; set; }

        [Metadata(FriendlyName = "String Array Property", Description = "This is a happy string[].")]
        public string[] StringArrayProperty { get; set; }

    }
}

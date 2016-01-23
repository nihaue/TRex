using TRex.Metadata;

namespace TRex.TestApiApp.Models
{
    public class VisibilityTestModel
    {

        [Metadata(Visibility = VisibilityType.Advanced)]
        public string Advanced { get; set; }

        [Metadata(Visibility = VisibilityType.Default)]
        public string Default { get; set; }

        [Metadata(Visibility = VisibilityType.Important)]
        public string Important { get; set; }

        [Metadata(Visibility = VisibilityType.Internal)]
        public string Internal { get; set; }

        public string NoAttribute { get; set; }

    }
}

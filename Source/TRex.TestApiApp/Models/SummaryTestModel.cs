using TRex.Metadata;

namespace TRex.TestApiApp.Models
{
    public class SummaryTestModel
    {

        [Metadata("Property friendly name", "Property description")]
        public string Property { get; set; }

    }
}

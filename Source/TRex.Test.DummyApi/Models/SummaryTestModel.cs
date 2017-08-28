using TRex.Metadata;

namespace TRex.Test.DummyApi.Models
{
    public class SummaryTestModel
    {

        [Metadata("Property friendly name", "Property description")]
        public string Property { get; set; }

    }
}

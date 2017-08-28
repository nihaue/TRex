namespace TRex.Metadata.Tests.TestDoubles
{
    public class DynamicModelHardcodedProp : DynamicModelBase
    {
        public string HardcodedProperty { get; set; }

        public string HardcodedNullProperty { get; set; }

        public DynamicModelHardcodedProp(object source) : base(source) { }

        public DynamicModelHardcodedProp() { }

    }
}

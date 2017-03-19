using TRex.Metadata;

namespace QuickLearn.SampleApi.Models
{
    public class ContactCreatedResponse
    {
        [Metadata("Name", "Name by which the contact can be looked up", VisibilityType.Important)]
        public string Name { get; set; }

        [Metadata("Result Message", "Message that explains what was stored", VisibilityType.Advanced)]
        public string Message { get; set; }
    }
}
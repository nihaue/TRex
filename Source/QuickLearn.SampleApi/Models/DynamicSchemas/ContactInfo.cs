using TRex.Metadata;
// using Newtonsoft.Json.Linq;
// using System;

namespace QuickLearn.SampleApi.Models
{
    [DynamicSchemaLookup("GetContactInfoSchema",
        valuePath: "Schema", parameters: "type={contactType}")]
    public class ContactInfo : DynamicModelBase
    {

        // These two constructors are MANDATORY for any dynamic model
        public ContactInfo(object source) : base(source) { }

        public ContactInfo() { }
        
        /* Examples of other nice things we could have here...
        
        // You can still add properties directly when using DynamicModelBase, just
        // make sure that you account for them in your schema

        private Guid id = Guid.NewGuid();
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        // Implicit conversions are nice to have around, but not mandatory

        public static implicit operator ContactInfo(JToken source)
        {
            return new ContactInfo(source);
        }

        */

    }
}

using System;

namespace TRex.Metadata
{

    [System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class ValueSourceAttribute : Attribute
    {

        public string Parameters { get; set; }

        public string LookupOperation { get; set; }

        public string ValuePath { get; set; }

        public string ValueCollection { get; set; }

        public string ValueTitle { get; set; }

        public ValueSourceAttribute()
        {

        }

        public ValueSourceAttribute(string lookupOperationId, string valueCollection, string valuePath, string valueTitle)
        {
            LookupOperation = lookupOperationId;
            ValueCollection = valueCollection;
            ValuePath = valuePath;
            ValueTitle = valueTitle;
        }

        public ValueSourceAttribute(string lookupOperationId, string parameters, string valueCollection, string valuePath, string valueTitle)
        {
            LookupOperation = lookupOperationId;
            Parameters = parameters;
            ValueCollection = valueCollection;
            ValuePath = valuePath;
            ValueTitle = valueTitle;
        }

    }
}
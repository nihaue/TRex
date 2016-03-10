using System;

namespace TRex.Metadata
{

    /// <summary>
    /// Provides a mechanism to emit the x-ms-dynamic-values vendor extension for a decorated parameter
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class DynamicValueLookupAttribute : Attribute
    {

        public string Parameters { get; set; }

        public string LookupOperation { get; set; }

        public string ValuePath { get; set; }

        public string ValueCollection { get; set; }

        public string ValueTitle { get; set; }

        public DynamicValueLookupAttribute()
        {

        }

        public DynamicValueLookupAttribute(string lookupOperationId, string valueCollection, string valuePath, string valueTitle)
        {
            LookupOperation = lookupOperationId;
            ValueCollection = valueCollection;
            ValuePath = valuePath;
            ValueTitle = valueTitle;
        }

        public DynamicValueLookupAttribute(string lookupOperationId, string parameters, string valueCollection, string valuePath, string valueTitle)
        {
            LookupOperation = lookupOperationId;
            Parameters = parameters;
            ValueCollection = valueCollection;
            ValuePath = valuePath;
            ValueTitle = valueTitle;
        }

    }
}
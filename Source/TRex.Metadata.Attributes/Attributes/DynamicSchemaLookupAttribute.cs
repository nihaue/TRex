using System;
using System.Net;

namespace TRex.Metadata
{

    /// <summary>
    /// Provides a mechanism to emit the x-ms-dynamic-schema vendor extension for a decorated parameter or method
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DynamicSchemaLookupAttribute : Attribute
    {
        public string Parameters { get; set; }

        public string LookupOperation { get; set; }

        public string ValuePath { get; set; }
        
        public DynamicSchemaLookupAttribute()
        {

        }
        
        public DynamicSchemaLookupAttribute(string lookupOperation, string valuePath, string parameters = null)
        {
            LookupOperation = lookupOperation;
            Parameters = parameters;
            ValuePath = valuePath;
        }
    }
}
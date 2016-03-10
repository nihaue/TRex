using System;
using System.Net;

namespace TRex.Metadata
{

    /// <summary>
    /// Provides a mechanism to emit the x-ms-dynamic-schema vendor extension for a decorated parameter or method
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ResponseTypeLookupAttribute : Attribute
    {
        
        public HttpStatusCode StatusCode { get; set; }

        public string Parameters { get; set; }

        public string LookupOperation { get; set; }

        public string ValuePath { get; set; }
        
        public ResponseTypeLookupAttribute()
        {

        }
        
        public ResponseTypeLookupAttribute(string lookupOperation, string valuePath, string parameters = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            LookupOperation = lookupOperation;
            Parameters = parameters;
            ValuePath = valuePath;
            StatusCode = statusCode;
        }
    }
}
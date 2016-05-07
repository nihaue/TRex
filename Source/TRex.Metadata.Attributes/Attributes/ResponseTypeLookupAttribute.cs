using System;
using System.Net;

namespace TRex.Metadata
{
    /// <summary>
    /// Provides a mechanism to emit the x-ms-dynamic-schema vendor extension for a decorated parameter or method
    /// </summary>
    /// <remarks>
    /// This is here as a place holder until the next version of Swashbuckle is released with extensions support for operation responses
    /// </remarks>
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class ResponseTypeLookupAttribute : Attribute
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
using System;

namespace TRex.Metadata
{

    /// <summary>
    /// Provides a mechanism to emit the x-ms-dynamic-values vendor extension for a decorated parameter
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class DynamicValueLookupAttribute : Attribute
    {

        /// <summary>
        /// Gets or sets the parameter values to pass to the lookup operation
        /// (e.g., lookupOpParam={paramNameFromThisOperation}&lookupOpParam2=hardcoded)
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// Gets or sets the operation Id of the operation that returns the possible values for the current parameter.
        /// If you use the Metadata attribute on the lookup operation, use a friendly name without
        /// spaces and then use the EXACT same name for this value.
        /// </summary>
        public string LookupOperation { get; set; }

        /// <summary>
        /// Gets or sets the name of the property that contains the value that will be used. For example in a list of countries,
        /// this would be the name of the property that contains a 2-character country code.
        /// </summary>
        public string ValuePath { get; set; }

        /// <summary>
        /// Gets or sets the name of the collection within the lookup operation's output that contains
        /// the possible values.
        /// </summary>
        public string ValueCollection { get; set; }

        /// <summary>
        /// Gets or sets the name of the property within lookup operation's output that contains
        /// a friendly name for the value that will be used. For example in a list of countries,
        /// this would be the name of the property that contains the name of the country.
        /// </summary>
        public string ValueTitle { get; set; }

        /// <summary>
        /// Initializes a new instance of the DynamicValueLookup attribute
        /// </summary>
        public DynamicValueLookupAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance of the DynamicValueLookup attribute using the information supplied
        /// </summary>
        /// <param name="lookupOperationId">Operation Id of the operation that returns the possible values for the current parameter.
        /// If you use the Metadata attribute on the lookup operation, use a friendly name without
        /// spaces and then use the EXACT same name for this value.</param>
        /// <param name="valueCollection">Name of the collection within the lookup operation's output that contains
        /// the possible values</param>
        /// <param name="valuePath">Name of the property that contains the value that will be used. For example in a list of countries,
        /// this would be the name of the property that contains a 2-character country code.</param>
        /// <param name="valueTitle">Name of the property that contains a friendly name for the value that will be used.
        /// For example in a list of countries, this would be the name of the property that contains
        /// the name of the country.</param>
        public DynamicValueLookupAttribute(string lookupOperationId, string valueCollection, string valuePath, string valueTitle)
        {
            LookupOperation = lookupOperationId;
            ValueCollection = valueCollection;
            ValuePath = valuePath;
            ValueTitle = valueTitle;
        }

        /// <summary>
        /// Initializes a new instance of the Metadata attribute using the information supplied
        /// </summary>
        /// <param name="lookupOperationId">Operation Id of the operation that returns the possible values for the current parameter.
        /// If you use the Metadata attribute on the lookup operation, choose a name without spaces and use
        /// the same name for this value</param>
        /// <param name="parameters">Parameter values to pass to the lookup operation
        /// (e.g., lookupOpParam={paramNameFromThisOperation}&lookupOpParam2=hardcoded)</param>
        /// <param name="valueCollection">Name of the collection within the lookup operation's output that contains
        /// the possible values</param>
        /// <param name="valuePath">Name of the property that contains the value that will be used. For example in a list of countries,
        /// this would be the name of the property that contains a 2-character country code.</param>
        /// <param name="valueTitle">Name of the property that contains a friendly name for the value that will be used.
        /// For example in a list of countries, this would be the name of the property that contains
        /// the name of the country.</param>
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
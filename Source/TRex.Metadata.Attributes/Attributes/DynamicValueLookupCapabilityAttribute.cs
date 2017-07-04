using System;

namespace TRex.Metadata
    {
    //TODO: finish descriptions
    /// <summary>
    /// Provides a mechanism to emit the x-ms-dynamic-values vendor extension with capability instead of operationId for a decorated parameter
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class DynamicValueLookupCapabilityAttribute : Attribute
        {
        /// <summary>
        /// Gets or sets capability name which returns possible values for the current parameter 
        /// </summary>
        public string Capability { get; set; }

        /// <summary>
        /// Gets or sets the parameters to pass to the capability
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// Gets or sets the name of the property from capability's output which contains the value which wil be used
        /// </summary>
        public string ValuePath { get; set; }

        /// <summary>
        /// (current behaviour) CURRENTLY DOESN'T WORK (MICROSOFT PLEASE) always tries to get Path parameter from the output
        /// and just crashes the Flow creating UI if it can't find Path field
        /// (expected behaviour) Gets or sets the name of the property from capability's output which will be shown in the flow UI
        /// </summary>
        public string ValueTitle { get; set; }

        /// <summary>
        /// Initializes a new instance of the DynamicValueLookup attribute using the information supplied
        /// </summary>
        /// <param name="capability">Name of capability</param>
        /// <param name="parameters">Initial parameters for capability</param>
        /// <param name="valuePath">Name of property that contains the value that will be used</param>
        /// <param name="valueTitle">Name of property that will be shown in MS Flow UI</param>
        public DynamicValueLookupCapabilityAttribute (string capability, string parameters, string valuePath, string valueTitle)
            {
            Capability = capability;
            Parameters = parameters;
            ValuePath = valuePath;
            ValueTitle = valueTitle;
            }
        }
    }
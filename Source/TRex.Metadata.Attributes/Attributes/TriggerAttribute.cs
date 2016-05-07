using System;

namespace TRex.Metadata
{

    /// <summary>
    /// Indicates that the given method is an operation that is either polled for data,
    /// or called to register a subscription for data, that will be used to either trigger
    /// the start or continuation of a flow
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class TriggerAttribute : Attribute
    {

        /// <summary>
        /// Constructs an instance of the trigger attribute for a single result polling trigger
        /// </summary>
        public TriggerAttribute()
        {
            Pattern = TriggerType.PollingSingle;
        }


        /// <summary>
        /// Constructs an instance of the trigger attribute for the current method
        /// </summary>
        /// <param name="pattern">Gets or sets the trigger input pattern implemented by the operation (i.e., poll operation for a single result to process,
        /// poll operation for a batch of results to process, or subscribe for an asynchronous notification)</param>
        /// <param name="dataType">Data type of the payload with which the flow is initiated / continued</param>
        /// <param name="dataFriendlyName">Friendly name and/or description of the payload with which the flow is initiated / continued</param>
        public TriggerAttribute(TriggerType pattern, Type dataType, string dataFriendlyName)
        {
            Pattern = pattern;
            DataType = dataType;
            DataFriendlyName = dataFriendlyName;
        }

        /// <summary>
        /// Gets or sets the friendly name of the payload with which the flow is initiated / continued
        /// </summary>
        public string DataFriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the data type of the payload with which the flow is initiated / continued
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// Gets or sets the trigger input pattern implemented by the operation (i.e., poll operation for a single result to process,
        /// poll operation for a batch of results to process, or subscribe for an asynchronous notification)
        /// </summary>
        public TriggerType Pattern { get; set; }
    }
}

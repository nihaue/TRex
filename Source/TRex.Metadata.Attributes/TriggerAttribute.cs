using System;

namespace TRex.Metadata
{


    /// <summary>
    /// Indicates that this action is either a polling action or push trigger callback registration.
    /// Polling triggers must have a query string parameter named triggerState.
    /// Push triggers must have a path parameter named triggerId.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method,  AllowMultiple = false)]
    public sealed class TriggerAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the TriggerAttribute based on the trigger type
        /// </summary>
        /// <param name="triggerType">Type of trigger implemented by the action in question, either Push or Poll</param>
        public TriggerAttribute(TriggerType triggerType)
        {
            TriggerType = triggerType;
        }

        /// <summary>
        /// Initializes a new instance of the TriggerAttribute based on the type of trigger and response type
        /// </summary>
        /// <param name="triggerType">Type of trigger implemented by the action in question, either Push or Poll</param>
        /// <param name="responseType">Type of data sent to the Logic App from the trigger. This is used to generate the correct body schema for the Logic App designer.</param>
        public TriggerAttribute(TriggerType triggerType, Type responseType)
            : this(triggerType)
        {
            if (responseType == null) return;

            ResponseType = responseType;
        }

        /// <summary>
        /// Gets or sets the type of trigger implemented by the action in question, either Push or Poll
        /// </summary>
        public TriggerType TriggerType { get; set; }

        /// <summary>
        /// Gets or sets the type of data sent to the Logic App from the trigger. This is used to generate the correct body schema for the Logic App designer.
        /// </summary>
        public Type ResponseType { get; set; }
    }
}

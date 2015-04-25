using System;

namespace TRex.Metadata
{
    [System.AttributeUsage(AttributeTargets.Method,  AllowMultiple = false)]
    public sealed class TriggerAttribute : Attribute
    {

        public TriggerAttribute(TriggerTypes triggerType = TriggerTypes.Poll, Type responseType = null)
        {
            TriggerType = triggerType;
            ResponseType = responseType;
        }

        public TriggerTypes TriggerType { get; set; }

        public Type ResponseType { get; set; }
    }
}

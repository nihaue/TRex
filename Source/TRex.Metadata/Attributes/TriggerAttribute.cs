using System;

namespace TRex.Metadata
{
    [System.AttributeUsage(AttributeTargets.Method,  AllowMultiple = false)]
    public sealed class TriggerAttribute : Attribute
    {

        public TriggerAttribute(TriggerType triggerType = TriggerType.Poll, Type responseType = null)
        {
            TriggerType = triggerType;
            ResponseType = responseType;
        }

        public TriggerType TriggerType { get; set; }

        public Type ResponseType { get; set; }
    }
}

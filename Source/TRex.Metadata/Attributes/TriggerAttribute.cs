using System;

namespace TRex.Metadata
{
    [System.AttributeUsage(AttributeTargets.Method,  AllowMultiple = false)]
    public sealed class TriggerAttribute : Attribute
    {

        public TriggerAttribute(TriggerType triggerType)
        {
            TriggerType = triggerType;
        }

        public TriggerAttribute(TriggerType triggerType, Type responseType)
            : this(triggerType)
        {
            if (responseType == null) return;

            ResponseType = responseType;
        }

        public TriggerType TriggerType { get; set; }

        public Type ResponseType { get; set; }
    }
}

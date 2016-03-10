using System;

namespace TRex.Metadata
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class TriggerAttribute : Attribute
    {

        public TriggerAttribute()
        {
            this.BatchMode = BatchMode.Single;
        }

        public TriggerAttribute(BatchMode batchMode)
        {
            this.BatchMode = batchMode;
        }

        public BatchMode BatchMode { get; set; }
    }
}

using System;

namespace TRex.Metadata
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class UnregisterCallbackAttribute : Attribute
    {
        public UnregisterCallbackAttribute()
        {
        }
    }

}

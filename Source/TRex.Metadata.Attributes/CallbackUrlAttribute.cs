using System;

namespace TRex.Metadata
{
    /// <summary>
    /// Indicates that this property should be populated with the callback URI of the Logic App at runtime.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class CallbackUrlAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the CallbackUrlAttribute
        /// </summary>
        public CallbackUrlAttribute()
        {

        }
    }
}

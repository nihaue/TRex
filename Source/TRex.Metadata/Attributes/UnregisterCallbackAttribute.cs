using System;

namespace TRex.Metadata
{

    /// <summary>
    /// Indicates that this action represents the action that should be called to 
    /// unregister a callback for a push trigger. The action must match the route
    /// of the callback registration and use the DELETE HTTP Verb. Additionally
    /// it must have a single path parameter named triggerId.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class UnregisterCallbackAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the UnregisterCallbackAttribute
        /// </summary>
        public UnregisterCallbackAttribute()
        {
        }
    }

}

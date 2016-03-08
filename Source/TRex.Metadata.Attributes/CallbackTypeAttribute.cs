using System;

namespace TRex.Metadata
{

    [System.AttributeUsage(AttributeTargets.Method, Inherited = true,
        AllowMultiple = false)]
    public sealed class CallbackTypeAttribute : Attribute
    {
        public CallbackTypeAttribute(Type callbackType, string friendlyName = null)
        {
            CallbackType = callbackType;

            FriendlyName = !string.IsNullOrWhiteSpace(friendlyName)
                                ? friendlyName
                                : callbackType.Name;
        }

        public Type CallbackType { get; set; }

        public string FriendlyName { get; set; }

    }
}

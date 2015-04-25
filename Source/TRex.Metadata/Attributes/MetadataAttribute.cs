using Swashbuckle.Swagger;
using System;
using System.Web.Http.Description;

namespace TRex.Metadata
{
    [System.AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Method,  AllowMultiple = false)]
    public sealed class MetadataAttribute : Attribute
    {
        public MetadataAttribute(string friendlyName = null,
            string description = null,
            VisibilityType visibility = VisibilityType.Default)
        {
            FriendlyName = friendlyName;
            Description = description;
            Visibility = visibility;
        }

        public string FriendlyName { get; set; }
        
        public string Description { get; set; }
        
        public VisibilityType Visibility { get; set; }

    }
}

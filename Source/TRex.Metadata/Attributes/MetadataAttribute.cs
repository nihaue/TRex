using Swashbuckle.Swagger;
using System;
using System.Web.Http.Description;

namespace TRex.Metadata
{

    /// <summary>
    /// Provides information about how to display this action,
    /// parameter, or model property within the Logic App designer.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Method,  AllowMultiple = false)]
    public sealed class MetadataAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the Metadata attribute using the information supplied
        /// </summary>
        /// <param name="friendlyName">Name of the item as it will be shown in the Logic App designer.
        /// For actions, this controls how the operation id is generated (pascal-cased form of the name supplied)</param>
        /// <param name="description">Brief description of the item to display in the Swagger UI</param>
        /// <param name="visibility">Visibility of the item in the Logic App designer. Default is visible, Advanced requires the user to click a button to reveal, and Internal hides the item.</param>
        public MetadataAttribute(string friendlyName = null,
            string description = null,
            VisibilityType visibility = VisibilityType.Default)
        {
            FriendlyName = friendlyName;
            Description = description;
            Visibility = visibility;
        }

        /// <summary>
        /// Gets or sets the name of the item as it will be shown in the Logic App designer.
        /// </summary>
        public string FriendlyName { get; set; }
        
        /// <summary>
        /// Gets or sets a brief description of the item to display in the Swagger UI.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the visibility of the item in the Logic App designer. Default is visible, Advanced requires the user to click a button to reveal, and Internal hides the item.
        /// </summary>
        public VisibilityType Visibility { get; set; }

    }
}

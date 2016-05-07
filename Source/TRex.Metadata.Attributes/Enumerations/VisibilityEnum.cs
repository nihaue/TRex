namespace TRex.Metadata
{
    /// <summary>
    /// Visibility of the item in the Logic App designer.
    /// Default is visible, Advanced requires the user to click
    /// a button to reveal, and Internal hides the item.
    /// </summary>
    public enum VisibilityType
    {
        /// <summary>
        /// The item is visible in the designer.
        /// </summary>
        Default,
        /// <summary>
        /// The item is hidden in the designer.
        /// </summary>
        Internal,
        /// <summary>
        /// The item is initially hidden in the designer, but the user can click a button to reveal it.
        /// </summary>
        Advanced,
        /// <summary>
        /// The item is highlighted in the designer.
        /// </summary>
        Important
    }
}

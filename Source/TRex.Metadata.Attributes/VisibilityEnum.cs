namespace TRex.Metadata
{
    /// <summary>
    /// Visibility of the item in the Logic App designer. Default is visible, Advanced requires the user to click a button to reveal, and Internal hides the item.
    /// </summary>
    public enum VisibilityType
    {
        Default,
        Internal,
        Advanced,
        Important
    }
}

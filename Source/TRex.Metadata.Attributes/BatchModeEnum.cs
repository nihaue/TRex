namespace TRex.Metadata
{
    /// <summary>
    /// Type of trigger that the operation represents. Specifies whether
    /// or not the operation can also serve as a trigger for a Logic App,
    /// and whether it returns a single value to be processed, or a batch
    /// (array) of values that should each individually trigger their own
    /// flow.
    /// </summary>
    public enum BatchMode
    {
            /// <summary>
            /// The operation returns a single item to be processed, and as such should trigger a single flow.
            /// </summary>
            Single,
            /// <summary>
            /// The operation returns an array to be processed. Each item in the array should trigger its own flow.
            /// </summary>
            Batched
    }
}

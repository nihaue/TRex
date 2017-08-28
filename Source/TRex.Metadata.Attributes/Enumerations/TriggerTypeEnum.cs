namespace TRex.Metadata
{
    /// <summary>
    /// Type of trigger that the operation represents. Specifies whether or not the operation
    /// can also initiate / continue a flow, and whether it returns a single value to be
    /// processed, a batch (array) of values that should each individually trigger / continue
    /// their own flow, or a subscription for an async notification that can trigger or continue
    /// a flow.
    /// </summary>
    public enum TriggerType
    {
            /// <summary>
            /// The operation represents a subscription for an async notification
            /// </summary>
            Subscription,
            /// <summary>
            /// The polling operation returns a single item to be processed, and as such should trigger a single flow.
            /// </summary>
            PollingSingle,
            /// <summary>
            /// The polling operation returns an array to be processed. Each item in the array should trigger its own flow.
            /// </summary>
            PollingBatched
    }
}

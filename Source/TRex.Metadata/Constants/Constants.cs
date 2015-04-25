namespace QuickLearn.ApiApps.Metadata
{

    /// <summary>
    /// Contains all of the required magical constants
    /// </summary>
    internal static class Constants
    {

        #region Microsoft Vendor Extensions

        /// <summary>
        /// Gets the string "x-ms-summary"
        /// </summary>
        internal const string X_MS_SUMMARY = "x-ms-summary";

        /// <summary>
        /// Gets the string "x-ms-scheduler-trigger"
        /// </summary>
        internal const string X_MS_SCHEDULER_TRIGGER = "x-ms-scheduler-trigger";

        /// <summary>
        /// Gets the string "x-ms-scheduler-recommendation"
        /// </summary>
        internal const string X_MS_SCHEDULER_RECOMMENDATION = "x-ms-scheduler-recommendation";

        /// <summary>
        /// Gets the string "x-ms-visibility"
        /// </summary>
        internal const string X_MS_VISIBILITY = "x-ms-visibility";

        #endregion

        #region Parameter Descriptions and Defaults

        /// <summary>
        /// Gets the string "triggerState"
        /// </summary>
        internal const string TRIGGER_STATE_PARAM_NAME = "triggerState";

        /// <summary>
        /// Gets the string "Trigger State"
        /// </summary>
        internal const string TRIGGER_STATE_PARAM_FRIENDLY_NAME = "Trigger State";

        /// <summary>
        /// Gets the string "@coalesce(triggers()?.outputs?.body?['triggerState'],'')"
        /// </summary>
        internal const string TRIGGER_STATE_MAGIC_DEFAULT = "@coalesce(triggers()?.outputs?.body?['triggerState'],'')";

        /// <summary>
        /// Gets the string triggerId
        /// </summary>
        internal const string TRIGGER_ID_PARAM_NAME = "triggerId";

        /// <summary>
        /// Gets the string "Trigger ID"
        /// </summary>
        internal const string TRIGGER_ID_PARAM_FRIENDLY_NAME = "Trigger ID";

        /// <summary>
        /// Gets the string "@workflow().name"
        /// </summary>
        internal const string TRIGGER_ID_MAGIC_DEFAULT = "@workflow().name";

        /// <summary>
        /// Gets the string "callbackUrl"
        /// </summary>
        internal const string CALLBACK_URL_PROPERTY_NAME = "callbackUrl";

        /// <summary>
        /// Gets the string "@accessKeys('default').primary.secretRunUri"
        /// </summary>
        internal const string CALLBACK_URL_MAGIC_DEFAULT = "@accessKeys('default').primary.secretRunUri";


        #endregion  

        #region Response Descriptions

        /// <summary>
        /// Gets the string "default"
        /// </summary>
        internal const string DEFAULT_RESPONSE_KEY = "default";

        /// <summary>
        /// Gets the response code "200"
        /// </summary>
        internal const string HAPPY_POLL_WITH_DATA_RESPONSE_CODE = "200";

        /// <summary>
        /// Gets the response code "202"
        /// </summary>
        internal const string HAPPY_POLL_NO_DATA_RESPONSE_CODE = "202";

        /// <summary>
        /// Gets the first digit ("2") of the 200 level of response codes
        /// </summary>
        internal const string HAPPY_RESPONSE_CODE_LEVEL_START = "2";
        

        #endregion

    }
}

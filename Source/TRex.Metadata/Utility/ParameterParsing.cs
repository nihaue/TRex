using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace QuickLearn.ApiApps.Metadata
{
    internal static class ParsingUtility
    {
        public static JObject ParseJsonOrUrlEncodedParams(string paramString)
        {

            if (string.IsNullOrWhiteSpace(paramString)) return new JObject();

            bool parseSuccess = false;
            JObject result = null;

            try
            {
                result = JObject.Parse(paramString);
                parseSuccess = true;
            }
            catch (JsonReaderException)
            {
            }

            if (!parseSuccess)
            {

                // This is a hack, and it feels bad.
                var parsedParams = UriExtensions.ParseQueryString(
                                                    new Uri(
                                                        string.Format(CultureInfo.CurrentCulture,
                                                            "http://tempuri.org/?{0}", paramString)));

                result = JObject.FromObject(parsedParams.AllKeys
                                            .ToDictionary(
                                                k => k,
                                                k => parsedParams[k]
                                            ));

                parseSuccess = true;
            }

            return result;
        }

    }
}

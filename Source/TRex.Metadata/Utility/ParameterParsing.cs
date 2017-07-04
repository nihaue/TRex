using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace QuickLearn.ApiApps.Metadata
{
    internal static class ParsingUtility
    {

        private const string PARAM_PLACEHOLDER = @"^{([\w\d]+)}$";


        private static void buildParameterPlaceholders(this JProperty currentProperty)
        {

            if (currentProperty == null) return;
            if (!currentProperty.HasValues) return;

            // Work recursively through the object
            if (currentProperty.Type == JTokenType.Object)
            {
                foreach (var prop in currentProperty.Value<JObject>().Properties())
                    prop.buildParameterPlaceholders();

                return;
            }

            var rawValue = currentProperty.Value.ToString();

            // There is no value at all
            if (string.IsNullOrWhiteSpace(rawValue)) return;

            var placeholderMatch = Regex.Match(rawValue, PARAM_PLACEHOLDER);

            // There is a value, but it's not a placeholder
            // If the value is true or false parse it as a bool
            if (placeholderMatch.Groups.Count != 2)
            {
                //changes true/false values from string to bool in json
                if (rawValue == "true")
                    currentProperty.Value = true;
                else if (rawValue == "false")
                    currentProperty.Value = false;
                return;
            }

        var placeholderName = placeholderMatch.Groups[1].Value;

            // There is no placeholder value to expand, even though it looked like there was
            if (string.IsNullOrWhiteSpace(placeholderName)) return;

            // Finally, there is a placeholder, so let's expand it
            JObject expandedValue = new JObject
            {
                { Constants.PARAMETER, placeholderName }
            };

            currentProperty.Value = expandedValue;

        }


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
            
            foreach (var prop in result.Properties())
                prop.buildParameterPlaceholders();

            return result;
        }

    }
}

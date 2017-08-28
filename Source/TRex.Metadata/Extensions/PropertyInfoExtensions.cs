using Newtonsoft.Json;
using System;
using System.Reflection;

namespace QuickLearn.ApiApps.Metadata.Extensions
{
    internal static class PropertyInfoExtensions
    {

        /// <summary>
        /// Gets the property name as it will appear in serialized form given the use of the JsonPropertyAttribute
        /// </summary>
        /// <param name="property">PropertyInfo for the property in question</param>
        /// <returns>Returns a string containing the serialized name of the property</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when PropertyInfo instance passed in is null</exception>
        public static string GetSerializedPropertyName(this PropertyInfo property)
        {

            if (property == null) throw new ArgumentNullException(nameof(property));

            var jsonPropertyInfo = property.GetCustomAttribute<JsonPropertyAttribute>(true);

            return null == jsonPropertyInfo || string.IsNullOrWhiteSpace(jsonPropertyInfo.PropertyName)
                        ? property.Name
                        : jsonPropertyInfo.PropertyName;

        }

    }
}

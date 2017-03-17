using QuickLearn.ApiApps.Metadata.Extensions;
using Swashbuckle.Swagger;
using System;
using System.Linq;
using System.Reflection;
using TRex.Metadata;
using TRex.Metadata.Models;

namespace QuickLearn.ApiApps.Metadata
{
    internal class TRexSchemaFilter : ISchemaFilter
    {

        public TRexSchemaFilter()
        {

        }

        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (schema == null || schema.properties == null || type == null) return;

            applySchemaLookupForDynamicModels(schema, type);

            foreach (var propertyName in schema.properties.Keys)
            {
                var schemaProperty = schema.properties[propertyName];
                var propertyInfo = type.GetRuntimeProperties().Where(p => p.Name == propertyName).FirstOrDefault();

                applyPropertyMetadata(schemaProperty, propertyInfo);
                applyCallbackUrl(schemaProperty, propertyInfo);
            }
        }


        private static void applySchemaLookupForDynamicModels(Schema schema, Type type)
        {
            if (schema == null || type == null) return;

            var dynamicSchemaInfo = type.GetCustomAttributes<DynamicSchemaLookupAttribute>().FirstOrDefault();

            if (null == dynamicSchemaInfo) return;

            var schemaLookupSettings = new DynamicSchemaModel();

            schemaLookupSettings.OperationId = dynamicSchemaInfo.LookupOperation;
            schemaLookupSettings.Parameters = ParsingUtility.ParseJsonOrUrlEncodedParams(dynamicSchemaInfo.Parameters);
            schemaLookupSettings.ValuePath = dynamicSchemaInfo.ValuePath;

            schema.SetSchemaLookup(schemaLookupSettings);
        }


        private static void applyCallbackUrl(Schema schemaProperty, PropertyInfo propertyInfo)
        {

            if (schemaProperty == null || propertyInfo == null) return;

            var callbackUrlAttribute = propertyInfo.GetCustomAttribute<CallbackUrlAttribute>();

            if (callbackUrlAttribute != null)
            {
                schemaProperty.SetCallbackUrl();
            }

        }

        private static void applyPropertyMetadata(Schema schemaProperty, PropertyInfo propertyInfo)
        {

            // Apply friendly names and descriptions wherever possible
            // "x-ms-summary" - friendly name (applies to properties)
            // schema.properties["prop"].description - description (applies to parameters)

            if (schemaProperty == null || propertyInfo == null) return;

            var propertyMetadata = propertyInfo.GetCustomAttribute<MetadataAttribute>();

            if (propertyMetadata != null)
            {
                schemaProperty.SetVisibility(propertyMetadata.Visibility);
                schemaProperty.SetFriendlyNameAndDescription(propertyMetadata.FriendlyName, propertyMetadata.Description);
            }

        }
    }


}

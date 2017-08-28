using QuickLearn.ApiApps.Metadata.Extensions;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (schema == null || type == null)
            {
                return;
            }

            applySchemaLookupForDynamicModels(schema, schemaRegistry, type);

            if (schema.properties == null)
            {
                return;
            }

            foreach (var propertyName in schema.properties.Keys)
            {
                var schemaProperty = schema.properties[propertyName];
                var propertyInfo = type.GetRuntimeProperties().Where(p => p.GetSerializedPropertyName() == propertyName).FirstOrDefault();

                applyPropertyMetadata(schemaProperty, propertyInfo);
                applyCallbackUrl(schemaProperty, propertyInfo);
            }
        }


        private static void applySchemaLookupForDynamicModels(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (schema == null || type == null) return;

            var dynamicSchemaInfo = type.GetCustomAttributes<DynamicSchemaLookupAttribute>().FirstOrDefault();

            if (null == dynamicSchemaInfo) return;

            var schemaLookupSettings = new DynamicSchemaModel()
            {
                OperationId = dynamicSchemaInfo.LookupOperation,
                Parameters = ParsingUtility.ParseJsonOrUrlEncodedParams(dynamicSchemaInfo.Parameters),
                ValuePath = dynamicSchemaInfo.ValuePath
            };

            // Swashbuckle should end up generating a ref schema in this case already,
            // in which case all that is necessary is to apply the vendor extensions and 
            // get out of this method
            if (type.BaseType == typeof(object))
            {
                schema.SetSchemaLookup(schemaLookupSettings);
                return;
            }

            // Determine if the dynamic schema already appears in the schema registry
            // if it appears, we will reference it's definition and make sure it has the
            // vendor extension applied 
            if (schemaRegistry.Definitions.ContainsKey(type.Name))
            {
                schemaRegistry.Definitions[type.Name].SetSchemaLookup(schemaLookupSettings);
            }
            else
            {
                // Dynamic schema hasn't been registered yet, let's do that to make sure
                // the schema doesn't get inlined (since the settings will be common for the type
                // given that the attribute appears at the class-level)
                var dynamicSchema = new Schema()
                {
                    additionalProperties = schema.additionalProperties,
                    allOf = schema.allOf,
                    @default = schema.@default,
                    description = schema.description,
                    discriminator = schema.discriminator,
                    @enum = schema.@enum,
                    example = schema.example,
                    exclusiveMaximum = schema.exclusiveMaximum,
                    exclusiveMinimum = schema.exclusiveMinimum,
                    externalDocs = schema.externalDocs,
                    format = schema.format,
                    items = schema.items,
                    maximum = schema.maximum,
                    maxItems = schema.maxItems,
                    maxLength = schema.maxLength,
                    maxProperties = schema.maxProperties,
                    minimum = schema.minimum,
                    minItems = schema.minItems,
                    minLength = schema.minLength,
                    minProperties = schema.minProperties,
                    multipleOf = schema.multipleOf,
                    pattern = schema.pattern,
                    properties = schema.properties,
                    readOnly = schema.readOnly,
                    @ref = schema.@ref,
                    required = schema.required,
                    title = schema.title,
                    type = schema.type,
                    uniqueItems = schema.uniqueItems,
                    xml = schema.xml
                };

                dynamicSchema.SetSchemaLookup(schemaLookupSettings);
                dynamicSchema.properties = dynamicSchema.properties ?? new Dictionary<string, Schema>();

                schemaRegistry.Definitions.Add(type.Name, dynamicSchema);


            }

            // Let's make sure the current schema points to the definition that is registered
            // and doesn't get inlined
            if (string.IsNullOrWhiteSpace(schema.@ref))
            {
                schema.@ref = $"#/{nameof(schemaRegistry.Definitions).ToLower(CultureInfo.InvariantCulture)}/{type.Name}";
                schema.type = null;
                schema.properties = null;
            }

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

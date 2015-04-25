using Microsoft.Azure.AppService.ApiApps.Service;
using QuickLearn.ApiApps.Metadata.Extensions;
using Swashbuckle.Swagger;
using System;
using System.Linq;
using System.Reflection;
using TRex.Metadata;

namespace QuickLearn.ApiApps.Metadata
{
    internal class TRexSchemaFilter : ISchemaFilter
    {

        public TRexSchemaFilter()
        {

        }

        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (schema.properties == null) return;

            bool isPushTrigger = type.AssemblyQualifiedNameNoTypeParams() == typeof(TriggerInput<string, string>).AssemblyQualifiedNameNoTypeParams();

            foreach (var propertyName in schema.properties.Keys)
            {
                var property = schema.properties[propertyName];

                if (isPushTrigger && propertyName == Constants.CALLBACK_URL_PROPERTY_NAME)
                {
                    #region Apply callback magic defaults

                    // Apply trigger magic defaults:
                    // "x-ms-scheduler-recommendation": "@accessKeys('default').primary.secretRunUri
                    schema.SetChildPropertyRequired(Constants.CALLBACK_URL_PROPERTY_NAME);

                    property.SetVisibility(VisibilityType.Internal);
                    property.SetSchedulerRecommendation(Constants.CALLBACK_URL_MAGIC_DEFAULT);

                    // This is what this will look like (pulled from HTTP Listener API Definition)
                    //
                    // "TriggerInput[TriggerPushParameters,TriggerOutputParameters]": {
                    //     "required": [            <-- SetChildPropertyRequired (on the parent model containing the callbackUrl property)
                    //       "callbackUrl"
                    // ],
                    // "type": "object",
                    // "properties": {
                    //   "callbackUrl": {            <-- SetSchedulerRecommendation (on the actual property)
                    //     "type": "string",
                    //     "x-ms-visibility": "internal",
                    //     "x-ms-scheduler-recommendation": "@accessKeys('default').primary.secretRunUri"
                    //   },

                    #endregion
                }

                // Apply friendly names and descriptions wherever possible
                // "x-ms-summary" - friendly name (applies to properties)
                // schema.properties["prop"].description - description (applies to parameters)

                var propertyInfo = type.GetRuntimeProperties().Where(p => p.Name == propertyName).FirstOrDefault();

                if (propertyInfo == null) return;

                var propertyMetadata = propertyInfo.GetCustomAttribute<MetadataAttribute>();

                if (propertyMetadata != null)
                {
                    property.SetVisibility(propertyMetadata.Visibility);
                    property.SetFriendlyNameAndDescription(propertyMetadata.FriendlyName, propertyMetadata.Description);
                }

            }
        }

    }


}

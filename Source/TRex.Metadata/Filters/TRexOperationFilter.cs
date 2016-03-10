using QuickLearn.ApiApps.Metadata.Extensions;
using Swashbuckle.Swagger;
using System;
using System.Linq;
using System.Web.Http.Description;
using TRex.Metadata;
using TRex.Metadata.Models;

namespace QuickLearn.ApiApps.Metadata
{
    internal class TRexOperationFilter : IOperationFilter
    {

        public TRexOperationFilter() { }

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {

            if (operation == null) return;

            applyUnregisterCallbackInfo(operation, apiDescription);

            applyDefaultResponse(operation);

            // Handle Metadata attribute
            applyOperationMetadataAndVisibility(operation, apiDescription);

            // Handle DynamicValueLookup attribute
            applyValueLookupForDynamicParameters(operation, apiDescription);

            // Handle DynamicSchemaLookup attribute
            applySchemaLookupForDynamicParameters(operation, apiDescription);

            // Handle CallbackType attribute
            applyCallbackType(operation, schemaRegistry, apiDescription);

        }

        private static void applyCallbackType(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {

            var callbackTypeInfoResult = apiDescription.ActionDescriptor.GetCustomAttributes<CallbackTypeAttribute>();
            var callbackTypeInfo = callbackTypeInfoResult == null ? null : callbackTypeInfoResult.FirstOrDefault();

            if (callbackTypeInfo == null) return;
            
            operation.SetCallbackType(schemaRegistry, callbackTypeInfo.CallbackType, callbackTypeInfo.FriendlyName);

        }

        private static void applySchemaLookupForDynamicParameters(Operation operation, ApiDescription apiDescription)
        {
            if (operation == null || apiDescription == null) return;

            var lookupParameters = from p in apiDescription.ParameterDescriptions
                                   let schemaLookupInfo = p.ParameterDescriptor.GetCustomAttributes<DynamicSchemaLookupAttribute>()
                                   where schemaLookupInfo != null
                                              && schemaLookupInfo.FirstOrDefault() != null
                                   select new
                                   {
                                       SwaggerParameter = operation.parameters.FirstOrDefault(param => param.name == p.Name),
                                       Parameter = p,
                                       SchemaLookupInfo = schemaLookupInfo.FirstOrDefault()
                                   };

            if (!lookupParameters.Any()) return;

            foreach (var param in lookupParameters)
            {
                var schemaLookup = new DynamicSchemaModel();

                schemaLookup.Parameters = ParsingUtility.ParseJsonOrUrlEncodedParams(param.SchemaLookupInfo.Parameters);
                schemaLookup.OperationId =
                    apiDescription.ResolveOperationIdForSiblingAction(
                        param.SchemaLookupInfo.LookupOperation,
                        schemaLookup.Parameters.Properties().Select(p => p.Name).ToArray());
                schemaLookup.ValuePath = param.SchemaLookupInfo.ValuePath;

                param.SwaggerParameter.SetSchemaLookup(schemaLookup);
            }
        }


        private static void applyValueLookupForDynamicParameters(Operation operation, ApiDescription apiDescription)
        {
            if (operation == null || apiDescription == null) return;

            var lookupParameters = from p in apiDescription.ParameterDescriptions
                                   let valueLookupInfo = p.ParameterDescriptor.GetCustomAttributes<DynamicValueLookupAttribute>()
                                   where valueLookupInfo != null
                                              && valueLookupInfo.FirstOrDefault() != null
                                   select new
                                   {
                                       SwaggerParameter = operation.parameters.FirstOrDefault(param => param.name == p.Name),
                                       Parameter = p,
                                       ValueLookupInfo = valueLookupInfo.FirstOrDefault()
                                   };

            if (!lookupParameters.Any()) return;

            foreach (var param in lookupParameters)
            {
                var valueLookup = new DynamicValuesModel();

                valueLookup.Parameters = ParsingUtility.ParseJsonOrUrlEncodedParams(param.ValueLookupInfo.Parameters);
                valueLookup.OperationId =
                    apiDescription.ResolveOperationIdForSiblingAction(
                        param.ValueLookupInfo.LookupOperation,
                        valueLookup.Parameters.Properties().Select(p => p.Name).ToArray());
                valueLookup.ValueCollection = param.ValueLookupInfo.ValueCollection;
                valueLookup.ValuePath = param.ValueLookupInfo.ValuePath;
                valueLookup.ValueTitle = param.ValueLookupInfo.ValueTitle;

                param.SwaggerParameter.SetValueLookup(valueLookup);
            }
        }

        /// <summary>
        /// Applies appropriate metadata for operation responsible for unregistering callbacks
        /// </summary>
        /// <param name="operation">Callback unregistration operation</param>
        /// <param name="apiDescription">Implementation metadata</param>
        private static void applyUnregisterCallbackInfo(Operation operation, ApiDescription apiDescription)
        {
            //var operationUnregisterCallbackInfoResult = apiDescription.ActionDescriptor.GetCustomAttributes<UnregisterCallbackAttribute>();
            //var operationUnregisterCallbackInfo = operationUnregisterCallbackInfoResult == null ? null : operationUnregisterCallbackInfoResult.FirstOrDefault();

            //if (operationUnregisterCallbackInfo == null) return;

            //operation.SetFriendlyNameAndDescription("Unregister Callback", "Unregisters the callback from being invoked when the event is triggered");
            //operation.SetVisibility(VisibilityType.Internal);

            // TODO: Apply 204 response type

        }

        /// <summary>
        /// Applies the friendly names, descriptions, and visibility settings to the operation
        /// </summary>
        /// <param name="operation">Exposed operation metadata</param>
        /// <param name="apiDescription">Implementation metadata</param>
        private static void applyOperationMetadataAndVisibility(Operation operation, ApiDescription apiDescription)
        {

            // Apply friendly names and descriptions where possible
            //      operation.summary - friendly name (applies to methods)
            //      operation.description - description (applies to methods)
            //      "x-ms-summary" - friendly name (applies to parameters and their properties)
            //      operation.parameters[x].description - description (applies to parameters)

            var operationMetadataResults = apiDescription.ActionDescriptor.GetCustomAttributes<MetadataAttribute>();
            var operationMetadata = operationMetadataResults == null ? null : operationMetadataResults.FirstOrDefault();

            if (operationMetadata != null)
            {
                operation.SetFriendlyNameAndDescription(operationMetadata.FriendlyName, operationMetadata.Description);
                operation.SetVisibility(operationMetadata.Visibility);
            }

            if (operation.parameters == null) return;

            // Ensure that we get the parameters of the operation all annotated appropriately as well
            applyOperationParameterMetadataAndVisibility(operation, apiDescription);

        }

        /// <summary>
        /// Applies the friendly names, descriptions, and visibility settings to all operation parameters
        /// </summary>
        /// <param name="operation">Exposed operation metadata</param>
        /// <param name="apiDescription">Implementation metadata</param>
        private static void applyOperationParameterMetadataAndVisibility(Operation operation, ApiDescription apiDescription)
        {
            var operationParameters = apiDescription.ActionDescriptor.GetParameters();

            if (operationParameters != null)
            {
                foreach (var parameter in apiDescription.ActionDescriptor.GetParameters())
                {
                    var parameterMetadataResults = parameter.GetCustomAttributes<MetadataAttribute>();
                    var parameterMetadata = parameterMetadataResults == null ? null : parameterMetadataResults.FirstOrDefault();

                    var operationParam = operation.parameters.FirstOrDefault(p => p.name == parameter.ParameterName);

                    if (operationParam != null && parameterMetadata != null)
                    {
                        operationParam.SetFriendlyNameAndDescription(parameterMetadata.FriendlyName, parameterMetadata.Description);
                        operationParam.SetVisibility(parameterMetadata.Visibility);
                    }
                }
            }
        }

        /// <summary>
        /// Ensures that each operation has a "default" response with a 200-level response code
        /// </summary>
        /// <param name="operation">Metadata for the operation</param>
        private static void applyDefaultResponse(Operation operation)
        {
            if (!operation.responses.ContainsKey(Constants.DEFAULT_RESPONSE_KEY))
            {
                var successCode = (from statusCode in operation.responses.Keys
                                   where statusCode.StartsWith("2",
                                    StringComparison.OrdinalIgnoreCase)
                                   select statusCode).FirstOrDefault();

                if (successCode != null)
                {
                    operation.responses.Add(Constants.DEFAULT_RESPONSE_KEY, operation.responses[successCode]);
                }
            }
        }

    }


}

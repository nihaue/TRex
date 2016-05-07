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
            
            // Handle Metadata attribute
            applyOperationMetadataAndVisibility(operation, apiDescription);

            // Handle DynamicValueLookup attribute
            applyValueLookupForDynamicParameters(operation, apiDescription);

            // Handle DynamicSchemaLookup attribute
            applySchemaLookupForDynamicParameters(operation, apiDescription);

            // Handle Trigger attribute
            applyTriggerBatchModeAndResponse(operation, schemaRegistry, apiDescription);

            // Handle ResponseTypeLookup attribute
            applyResponseTypeLookup(operation, apiDescription);

            // Apply default response (copy 200 level response if available as "default")
            applyDefaultResponse(operation);

        }

        private static void applyResponseTypeLookup(Operation operation, ApiDescription apiDescription)
        {
            var responseTypeLookups = apiDescription.ActionDescriptor.GetCustomAttributes<ResponseTypeLookupAttribute>();

            foreach (var responseTypeLookupInfo in responseTypeLookups)
            { 
                var schemaLookup = new DynamicSchemaModel();

                schemaLookup.Parameters = ParsingUtility.ParseJsonOrUrlEncodedParams(responseTypeLookupInfo.Parameters);
                schemaLookup.OperationId = apiDescription.ResolveOperationIdForSiblingAction(
                                                responseTypeLookupInfo.LookupOperation,
                                                schemaLookup.Parameters.Properties().Select(p => p.Name).ToArray());
                schemaLookup.ValuePath = responseTypeLookupInfo.ValuePath;

                operation.SetResponseTypeLookup(responseTypeLookupInfo.StatusCode, schemaLookup);
            }
        }

        private static void applyTriggerBatchModeAndResponse(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var triggerInfo = apiDescription.ActionDescriptor.GetFirstOrDefaultCustomAttribute<TriggerAttribute>();

            if (triggerInfo == null) return;

            operation.SetTrigger(schemaRegistry, triggerInfo);
        }
        
        private static void applySchemaLookupForDynamicParameters(Operation operation, ApiDescription apiDescription)
        {
            if (operation == null || apiDescription == null) return;

            var dynamicSchemaParameters = from p in apiDescription.ParameterDescriptions
                                    let schemaLookupInfo = p.ParameterDescriptor.GetFirstOrDefaultCustomAttribute<DynamicSchemaLookupAttribute>()
                                    where schemaLookupInfo != null
                                   select new
                                   {
                                       SwaggerParameter = operation.parameters.FirstOrDefault(param => param.name == p.Name),
                                       Parameter = p,
                                       SchemaLookupInfo = schemaLookupInfo
                                   };

            if (!dynamicSchemaParameters.Any()) return;

            foreach (var param in dynamicSchemaParameters)
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
                                   let valueLookupInfo = p.ParameterDescriptor.GetFirstOrDefaultCustomAttribute<DynamicValueLookupAttribute>()
                                   where valueLookupInfo != null
                                   select new
                                   {
                                       SwaggerParameter = operation.parameters.FirstOrDefault(param => param.name == p.Name),
                                       Parameter = p,
                                       ValueLookupInfo = valueLookupInfo
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
            
            var operationMetadata = apiDescription.ActionDescriptor.GetFirstOrDefaultCustomAttribute<MetadataAttribute>();

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
                    var parameterMetadata = parameter.GetFirstOrDefaultCustomAttribute<MetadataAttribute>();

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

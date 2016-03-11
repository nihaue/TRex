using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using TRex.Metadata;

namespace QuickLearn.ApiApps.Metadata.Extensions
{
    internal static class ApiDescriptionExtensions
    {

        internal static string ResolveOperationIdForSiblingAction(this ApiDescription apiDescription, string actionName, string[] parameterNames)
        {
            string operationId = actionName;

            var matchingMethod = (from method in apiDescription.ActionDescriptor
                                                    .ControllerDescriptor.ControllerType
                                                    .GetMethods()
                                  let parameters = method.GetParameters()
                                                            .Select(p => p.Name)
                                                            .OrderBy(p => p)
                                                            .ToArray()
                                  where method.Name == actionName
                                        && parameters.SequenceEqual(parameterNames)
                                  select method).FirstOrDefault();

            if (matchingMethod != null)
            {

                var methodAttributes = matchingMethod.GetCustomAttributes(typeof(MetadataAttribute), true);

                MetadataAttribute methodMetadata = methodAttributes != null ? (MetadataAttribute)methodAttributes.FirstOrDefault() : null;

                if (methodMetadata != null)
                    operationId = OperationExtensions.GetOperationId(methodMetadata.FriendlyName);

            }

            return operationId;
        }

        internal static T GetFirstOrDefaultCustomAttribute<T>(this HttpActionDescriptor actionDescriptor) where T : Attribute
        {
            var attributeInfoResult = actionDescriptor.GetCustomAttributes<T>();
            return attributeInfoResult == null ? null : attributeInfoResult.FirstOrDefault();
        }

        internal static T GetFirstOrDefaultCustomAttribute<T>(this HttpParameterDescriptor parameterDescriptor) where T : Attribute
        {
            var attributeInfoResult = parameterDescriptor.GetCustomAttributes<T>();
            return attributeInfoResult == null ? null : attributeInfoResult.FirstOrDefault();
        }
    }
}

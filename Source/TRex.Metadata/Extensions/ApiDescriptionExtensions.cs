using System.Linq;
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

    }
}

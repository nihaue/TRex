using QuickLearn.ApiApps.Metadata;
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Web.Http.Description;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using TRex.Metadata.Models;

namespace TRex.Metadata
{
    internal class TRexDocumentFilter : IDocumentFilter
    {
        public TRexDocumentFilter() { }

        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            if (swaggerDoc == null) return;

            // This iterates through the paths and "moves up" any x-ms-notification-content
            // vendor extension values to the path level where the designer expects them to live
            foreach (var path in swaggerDoc.paths.Keys)
            {
                var currentPath = swaggerDoc.paths[path];

                if (null != currentPath?.post?.vendorExtensions
                    && currentPath.post.vendorExtensions.ContainsKey(Constants.X_MS_NOTIFICATION_CONTENT))
                {
                    if (null == currentPath.vendorExtensions)
                    {
                        currentPath.vendorExtensions = new Dictionary<string, object>();
                    }

                    currentPath.vendorExtensions[Constants.X_MS_NOTIFICATION_CONTENT] =
                        currentPath.post.vendorExtensions[Constants.X_MS_NOTIFICATION_CONTENT];
                    currentPath.post.vendorExtensions.Remove(Constants.X_MS_NOTIFICATION_CONTENT);
                }
            }

        }
    }
}
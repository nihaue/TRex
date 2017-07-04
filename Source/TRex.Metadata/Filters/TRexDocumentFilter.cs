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
        private FilePickerCapabilityModel capability;
        public TRexDocumentFilter()
        {

        }

        public TRexDocumentFilter(FilePickerCapabilityModel capability)
        {
            this.capability = capability;
        }

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

                    currentPath.vendorExtensions[Constants.X_MS_NOTIFICATION_CONTENT] = currentPath.post.vendorExtensions[Constants.X_MS_NOTIFICATION_CONTENT];
                    currentPath.post.vendorExtensions.Remove(Constants.X_MS_NOTIFICATION_CONTENT);
                }
            }

            if (capability == null) return;
            
            //handles file-picker capability (only 1 till microsoft gives more info about capabilities)
            //TODO: think if you need to check if the operation exists, Flow doesn't break with bad capabilities
            HandleCapabilities (swaggerDoc, apiExplorer, capability);
        }

        private static void HandleCapabilities (SwaggerDocument swaggerDoc, IApiExplorer apiExplorer, FilePickerCapabilityModel capability)
            {
            if (!swaggerDoc.vendorExtensions.ContainsKey (Constants.X_MS_CAPABILITIES))
                swaggerDoc.vendorExtensions.Add (
                    Constants.X_MS_CAPABILITIES,
                    new Dictionary<string, object> ()
                );

            ((Dictionary<string, object>) swaggerDoc.vendorExtensions.First(x => x.Key == Constants.X_MS_CAPABILITIES).Value)
                    .Add(Constants.FILE_PICKER, capability);
            }
    }
}
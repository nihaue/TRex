using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TRex.Metadata;

namespace QuickLearn.SampleApi.Formatters
{
    public class DynamicXmlFormatter : MediaTypeFormatter
    {
        public DynamicXmlFormatter()
        {
            SupportedMediaTypes.Add(
                new MediaTypeHeaderValue("application/xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(System.Net.Mime.MediaTypeNames.Text.Xml));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type.BaseType == typeof(DynamicModelBase);
        }

        public override Task WriteToStreamAsync(Type type, object value,
            Stream writeStream, System.Net.Http.HttpContent content,
            System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                var outputObject = new JObject();
                outputObject.Add(type.Name, value == null ? null : JToken.FromObject(value));

                var jsonString = JsonConvert.SerializeObject(outputObject);

                var xmlDocument = JsonConvert.DeserializeXmlNode(jsonString);
                
                xmlDocument.Save(writeStream);
            });
        }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using QuickLearn.SampleApi.Models;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Http;
using TRex.Metadata;

namespace QuickLearn.SampleApi.Controllers
{
    [RoutePrefix("api/contacts")]
    public class DynamicSchemasController : ApiController
    {

        [HttpGet,Route("{name}")]
        [Metadata("Get Contact Info", "Gets contact info of the specified type", VisibilityType.Important)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ContactInfo))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Invalid type specified")]
        public IHttpActionResult GetContactInfo(
            [Metadata("Contact Name")]string name,
            [Metadata("Contact Type", @"Try either ""Phone"" or ""Email""")]string contactType)
        {
            JObject result = new JObject();

            switch (contactType)
            {
                case "Phone":
                    result.Add("countryCode", "1");
                    result.Add("areaCode", "425");
                    result.Add("phoneNumber", "5555555");
                    break;
                case "Email":
                    result.Add("localPart", name);
                    result.Add("hostPart", "example.org");
                    result.Add("displayName", $"{name}@example.org");
                    break;
                default:
                    result = null;
                    break;
            }

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Unknown contact type.");
            }
        }


        [HttpGet,Route("schema")]
        [Metadata("GetContactInfoSchema", Visibility = VisibilityType.Internal)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(JObject))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Invalid type specified")]
        public IHttpActionResult GetSchema(string type)
        {
            var result = new JObject();
            var schema = new JsonSchema()
            {
                Title = type,
                Properties = new Dictionary<string, JsonSchema>(),
                Type = JsonSchemaType.Object
            };

            if (type == "Phone")
            {
                schema.Properties.Add("countryCode", new JsonSchema() { Type = JsonSchemaType.String });
                schema.Properties.Add("areaCode", new JsonSchema() { Type = JsonSchemaType.String });
                schema.Properties.Add("phoneNumber", new JsonSchema() { Type = JsonSchemaType.String });
            }
            else if (type == "Email")
            {
                schema.Properties.Add("localPart", new JsonSchema() { Type = JsonSchemaType.String });
                schema.Properties.Add("hostPart", new JsonSchema() { Type = JsonSchemaType.String });
                schema.Properties.Add("displayName", new JsonSchema() { Type = JsonSchemaType.String });
            }
            else
            {
                return BadRequest("Unknown contact type.");
            }
            
            var builder = new StringBuilder();
            var writer = new JsonTextWriter(new StringWriter(builder)) { Formatting = Formatting.None };

            // Removes nodes with default values before emitting
            schema.WriteTo(writer);
            writer.Flush();

            result.Add("Schema", JToken.Parse(builder.ToString()));
            return Ok(result);

        }


    }
}

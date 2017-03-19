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
        private const string LOOKUP_ROUTE = "ContactLookup";
        private static Dictionary<string, ContactInfo> phones = new Dictionary<string, ContactInfo>();
        private static Dictionary<string, ContactInfo> emails = new Dictionary<string, ContactInfo>();
        
        /// <summary>
        /// Looks up a contact by name. Demonstrates how to return an object with a dynamic schema.
        /// </summary>
        /// <param name="name">Name of the contact to lookup</param>
        /// <param name="contactType">Type of the contact to lookup. This parameter determines the output schema</param>
        /// <returns>Returns a ContactInfo instance using the schema specified by the contactType parameter.</returns>
        /// <remarks>
        /// All of the magic that makes the schema dynamic is within the ContactInfo type.
        /// It's a type that has the DynamicSchemaLookup attribute and a base class of DynamicModelBase
        ///
        /// See that type for an example of how to craft your own object that will support a dynamic schema. 
        /// </remarks>
        [HttpGet, Route("{name}", Name = LOOKUP_ROUTE)]
        [Metadata("Get Contact Info", "Gets contact info of the specified type", VisibilityType.Important)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ContactInfo))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Invalid type specified")]
        public IHttpActionResult GetContactInfo(
            [Metadata("Contact Name")]string name,
            [Metadata("Contact Type", @"Try either ""Phone"" or ""Email"""),]string contactType)
        {
            ContactInfo result = null;
            
            switch (contactType)
            {
                case "Phone":
                    result = lookupPhone(name);
                    break;
                case "Email":
                    result = lookupEmail(name);
                    break;
                default:
                    result = null;
                    break;
            }

            if (null != result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Unknown contact type.");
            }
        }

        private static ContactInfo lookupEmail(string name)
        {
            // Looking up the contact in the phones dictionary (in-memory).
            // If it has disappeared and/or was never inserted, we will
            // return a dummy object for demo purposes. In real life, this would
            // be a great place to have a 404 response

            ContactInfo result;
            if (emails.ContainsKey(name))
            {
                result = emails[name];
            }
            else
            {
                // Example of creating the result from an anonymous type
                result = new ContactInfo(new
                {
                    localPart = name,
                    hostPart = "example.org",
                    displayName = $"{name}@example.org"
                });
            }

            return result;
        }

        private static ContactInfo lookupPhone(string name)
        {
            ContactInfo result;
            if (phones.ContainsKey(name))
            {
                result = phones[name];
            }
            else
            {
                result = new ContactInfo(new
                {
                    countryCode = "1",
                    areaCode = "425",
                    phoneNumber = "5555555"
                });
            }

            return result;
        }


        /// <summary>
        /// Creates a contact of the specified type with the specified name. Demonstrates how to use a dynamic object as
        /// an operation input.
        /// </summary>
        /// <param name="name">Name of the contact to create</param>
        /// <param name="contactType">Type of the contact to create. This parameter determines the schema for the next parameter.</param>
        /// <param name="contact">Contact to create. Assumed to conform to the schema, but is treated as dynamic by the code.</param>
        /// <returns>Returns a message constructed from members of the created contact to demonstrate member access.</returns>
        [HttpPost, Route()]
        [Metadata("Insert Contact", "Inserts a new contact", VisibilityType.Advanced)]
        [SwaggerResponse(HttpStatusCode.Created, Type = typeof(ContactCreatedResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Invalid type specified")]
        public IHttpActionResult CreateNewContact(
            [Metadata("Contact Name")]string name,
            [Metadata("Contact Type", @"Try either ""Phone"" or ""Email""")]string contactType,
            [Metadata("Contact")][FromBody]ContactInfo contact)
        {

            // Since ContactInfo uses DynamicModelBase, it can be used as either
            // a dynamic value, or as a JToken. While dynamic is convenient,
            // JToken is likely the safer route to take.
            dynamic dynamicContact = contact;
            JToken jTokenContact = contact;

            var response = new ContactCreatedResponse() { Name = name };

            switch (contactType)
            {
                case "Phone":
                    // This demonsrates dynamic access to the object
                    phones[name] = contact;
                    response.Message = $"Added phone with country code {dynamicContact.countryCode}, area code {dynamicContact.areaCode}, and number {dynamicContact.phoneNumber}";
                    break;
                case "Email":
                    // This demonstrates accessing the object as a JToken
                    emails[name] = contact;
                    var displayName = jTokenContact["displayName"];
                    var localPart = jTokenContact["localPart"];
                    var hostPart = jTokenContact["hostPart"];
                    response.Message = $"Added email {displayName}, local part of {localPart}, and host part of {hostPart}";
                    break;
                default:
                    break;
            }

            if (string.IsNullOrWhiteSpace(response.Message))
            {
                return BadRequest("Unknown contact type");
            }

            return CreatedAtRoute<object>(LOOKUP_ROUTE, new { name = name, contactType = contactType }, response);

        }


        /// <summary>
        /// Generates a schema for the given contact type
        /// </summary>
        /// <param name="type">Type of contact for which to generate a schema</param>
        /// <returns></returns>
        [HttpGet, Route("schema")]
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

            /*
            
            // If you wanted to experiment with adding properties to the ContactInfo class directly,
            // that would appear to both Phone and Email contacts, you can. However, if you do add
            // any, you should account for them in the schema as well. Example is shown below:

            schema.Properties.Add("Id", new JsonSchema() { Type = JsonSchemaType.String });

            */

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

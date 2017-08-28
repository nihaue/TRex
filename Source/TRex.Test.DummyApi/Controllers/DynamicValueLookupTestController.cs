using Newtonsoft.Json.Linq;
using System.Web.Http;
using TRex.Metadata;
using TRex.Test.DummyApi.Models;

namespace TRex.Test.DummyApi.Controllers
{
	[RoutePrefix("test/x-ms-dynamic-values")]
	public class DynamicValueLookupTestController : ApiController
	{
		[HttpGet]
		[Route("friendly-operation-as-source")]
		public IHttpActionResult FriendlyAsSource(
				// T-Rex will reflect this method to lookup operation id
				[DynamicValueLookup(nameof(FriendlySource),
								// This will be parsed and re-created as JSON
								parameters: "sampleParam1={noAttributeParameter}&sampleParam2=hardcoded-value-here",
								valueCollection: "ItemList",
								valuePath: "Id",
								valueTitle: "DisplayName")]
				string lookupParameter,
				string noAttributeParameter)
		{
			return Ok();
		}

        [HttpPost]
        [Route("create-company")]
        public IHttpActionResult CreateCompany(DynamicValueLookupModel company)
        {
            return Ok();
        }

        [HttpGet]
        [Route("countries")]
        public IHttpActionResult GetCountries()
        {
            var result = new
            {
                Countries = new[] {
                    new
                    {
                        Id = "US",
                        Name = "United States of America"
                    },
                    new
                    {
                        Id = "CA",
                        Name = "Canada"
                    }
                }
            };

            return Ok(result);
        }

        [HttpGet]
		[Route("no-attribute-operation-as-source")]
		public IHttpActionResult NoAttributeAsSource(
				// T-Rex will reflect this method to lookup operation id
				[DynamicValueLookup(nameof(NoAttributeSource),
										// This will be parsed and re-created as JSON
										parameters: "sampleParam1=hardcoded",
										valueCollection: "ItemList",
										valuePath: "Id",
										valueTitle: "DisplayName")]
				string lookupParameter,
		string noAttributeParameter)
		{
			return Ok();
		}

		[HttpGet]
		[Route("literal-operationid-json-parameters")]
		public IHttpActionResult LiteralOperationIdJsonParameters(
						[DynamicValueLookup(lookupOperationId: "Some_Other_OperationId",
												parameters: @"{ ""sampleParam1"": ""{noAttributeParameter}"", ""sampleParam2"": ""hardcoded-value"" }",
												valueCollection: "ItemList",
												valuePath: "Id",
												valueTitle: "DisplayName")]
										string lookupParameter,
								string noAttributeParameter)
		{
			return Ok();
		}

		[HttpGet]
		[Route("literal-operationid-null-parameter")]
		public IHttpActionResult LiteralOperationIdNullParameter(
				[DynamicValueLookup(lookupOperationId: "Some_Other_OperationId",
								parameters: "sampleParam1=",
								valueCollection: "ItemList",
								valuePath: "Id",
								valueTitle: "DisplayName")]
						string lookupParameter,
						string noAttributeParameter)
		{
			return Ok();
		}

		[HttpGet]
		[Route("literal-operationid-no-parameters")]
		public IHttpActionResult LiteralOperationIdNoParameters(
		[DynamicValueLookup(lookupOperationId: "Some_Other_OperationId",
								valueCollection: "ItemList",
								valuePath: "Id",
								valueTitle: "DisplayName")]
						string lookupParameter,
				string noAttributeParameter)
		{
			return Ok();
		}


		[Metadata("Friendly Name For Operation")]
		[HttpGet]
		[Route("friendly-source")]
		public IHttpActionResult FriendlySource(string sampleParam1, string sampleParam2)
		{
			return Ok();
		}

		[HttpGet]
		[Route("no-attribute-source")]
		public IHttpActionResult NoAttributeSource(string sampleParam1)
		{
			return Ok();
		}

	}
}

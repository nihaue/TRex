using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRex.Metadata;

namespace TRex.Test.DummyApi.Controllers
{
    [RoutePrefix("test/x-ms-dynamic-values/capability")]
    public class DynamicValueLookupCapabilityTestController : ApiController
    {
        [HttpGet]
        [Route("capability-normal-parameters")]
        public IHttpActionResult CapabilityNormalParameters (
            [DynamicValueLookupCapability(capability: "Some_Capability",
                parameters: "sampleParam1={noAttributeParameter}&sampleParam2=hardcoded-value&sampleParam3=true",
                valuePath: "Id",
                valueTitle: "DisplayName")]
            string lookupParameter,
            string noAttributeParameter)
            {
            return Ok();
            }

        [HttpGet]
        [Route("capability-json-parameters")]
        public IHttpActionResult CapabilityJsonParameters (
            [DynamicValueLookupCapability(capability: "Some_Capability",
                parameters: @"{ ""sampleParam1"": ""{noAttributeParameter}"", ""sampleParam2"": ""hardcoded-value"", ""sampleParam3"": ""true"" }",
                valuePath: "Id",
                valueTitle: "DisplayName")]
            string lookupParameter,
            string noAttributeParameter)
            {
            return Ok();
            }

        [HttpGet]
        [Route("capability-no-parameter-value")]
        public IHttpActionResult CapabilityNoParameterValue (
            [DynamicValueLookupCapability(capability: "Some_Capability",
                parameters: "sampleParam1=",
                valuePath: "Id",
                valueTitle: "DisplayName")]
            string lookupParameter,
            string noAttributeParameter)
            {
            return Ok();
            }

        [HttpGet]
        [Route("capability-null-parameters")]
        public IHttpActionResult CapabilityNullParameters (
            [DynamicValueLookupCapability(capability: "Some_Capability",
                parameters: null,
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
        public IHttpActionResult FriendlySource (string sampleParam1, string sampleParam2)
            {
            return Ok();
            }

        }
}

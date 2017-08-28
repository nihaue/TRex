using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TRex.Test.Infrastructure;

namespace TRex.Metadata.Tests
{
    [TestClass]
    public class JsonPropertyTests
    {

        public JToken Swagger = JToken.Parse(SwaggerResolver.Swagger);

        [TestMethod, TestCategory("x-ms-summary"), TestCategory("Property Attribute"), TestCategory("JsonProperty")]
        public void JsonProperty_CustomSummary_SummaryApplied()
        {

            var propertySummaryNode = Swagger.SelectToken(@"definitions.JsonPropertyModel.properties.sampleProperty.x-ms-summary");

            Assert.IsNotNull(propertySummaryNode, "Property summary not found on property where JsonProperty attribute was used to define custom property name");

            var propertySummaryValue = propertySummaryNode.Value<string>();

            Assert.AreEqual("Sample Property",
                propertySummaryValue,
                "Incorrect property summary found on property named using JsonProperty attribute");
        }

        [TestMethod, TestCategory("description"), TestCategory("Property Attribute"), TestCategory("JsonProperty")]
        public void JsonProperty_CustomDescription_DescriptionApplied()
        {

            var propertyDescriptionNode = Swagger.SelectToken(@"definitions.JsonPropertyModel.properties.sampleProperty.description");

            Assert.IsNotNull(propertyDescriptionNode, "Property summary not found on property where JsonProperty attribute was used to define custom property name");

            var propertyDescriptionValue = propertyDescriptionNode.Value<string>();

            Assert.AreEqual("This is the description",
                propertyDescriptionValue,
                "Incorrect property description found on property named using JsonProperty attribute");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Property Attribute"), TestCategory("JsonProperty")]
        public void JsonProperty_CustomVisibility_VisibilityApplied()
        {

            var propertyVisibilityNode = Swagger.SelectToken(@"definitions.JsonPropertyModel.properties.sampleProperty.x-ms-visibility");

            Assert.IsNotNull(propertyVisibilityNode, "Property summary not found on property where JsonProperty attribute was used to define custom property name");

            var propertyVisibilityValue = propertyVisibilityNode.Value<string>();

            Assert.AreEqual("advanced",
                propertyVisibilityValue,
                "Incorrect property visibility found on property named using JsonProperty attribute");
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TRex.TestHelpers;

namespace TRex.Metadata.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class SummaryTests
    {

        public JToken Swagger = JToken.Parse(SwaggerResolver.Swagger);
        

        #region Method Attributes

        [TestMethod, TestCategory("operationId"), TestCategory("Method Attribute")]
        public void Method_FriendlyNameSet_OperationIdStartsWithFriendlyName()
        {
            var operationIdNode = Swagger.SelectToken(@"paths./test/x-ms-summary/operation.post.operationId");
            var operationIdValue = operationIdNode.Value<string>();

            StringAssert.StartsWith(operationIdValue, "OperationFriendlyName",
                "Friendly name not applied correctly in operationId node of the swagger metadata.");
        }

        [TestMethod, TestCategory("summary"), TestCategory("Method Attribute")]
        public void Method_FriendlyNameSet_SummarySetToFriendlyName()
        {
            var summaryNode = Swagger.SelectToken(@"paths./test/x-ms-summary/operation.post.summary");
            var summaryValue = summaryNode.Value<string>();

            Assert.AreEqual("Operation friendly name", summaryValue,
                "Friendly name not applied correctly in operation's summary node of the swagger metadata.");
        }
       
        [TestMethod, TestCategory("description"), TestCategory("Method Attribute")]
        public void Method_DescriptionSet_DescriptionInSwaggerIsSet()
        {
            var descriptionNode = Swagger.SelectToken(@"paths./test/x-ms-summary/operation.post.description");
            var descriptionValue = descriptionNode.Value<string>();

            Assert.AreEqual("Operation description", descriptionValue,
                "Description not applied correctly in operation's description node of the swagger metadata.");

        }

        [TestMethod, TestCategory("description"), TestCategory("summary"), TestCategory("Method Attribute")]
        public void Method_NoAttributesSet_NoSwaggerCustomizationsApplied()
        {
            var operationNode = Swagger.SelectToken(@"paths./test/x-ms-summary/no-attribute.post");

            Assert.IsNotNull(operationNode, "Route for test of operation without attributes does not exist.");

            var descriptionNode = operationNode.SelectToken(@"description");

            Assert.IsNull(descriptionNode, "Description was set for operation without any attributes.");

            var summaryNode = operationNode.SelectToken(@"summary");

            Assert.IsNull(summaryNode, "Summary was set for operation without any attributes.");
            
        }

        #endregion

        #region Parameter Attributes

        [TestMethod, TestCategory("x-ms-summary"), TestCategory("Parameter Attribute")]
        public void Parameter_FriendlyNameSet_FriendlyNameShownInSwagger()
        {
            var parameterSummaryNode = Swagger.SelectToken(@"paths./test/x-ms-summary/operation.post.parameters[?(@.name=='parameter')].x-ms-summary");

            var parameterSummaryValue = parameterSummaryNode.Value<string>();

            Assert.AreEqual("Parameter friendly name",
                parameterSummaryValue,
                "Parameter friendly name attribute did not cause x-ms-summary to be set correctly in the swagger metadata.");
        }

        [TestMethod, TestCategory("description"), TestCategory("Parameter Attribute")]
        public void Parameter_DescriptionSet_DescriptionShownInDescription()
        {
            var parameterDescriptionNode = Swagger.SelectToken(@"paths./test/x-ms-summary/operation.post.parameters[?(@.name=='parameter')].description");

            var parameterDescriptionValue = parameterDescriptionNode.Value<string>();

            Assert.AreEqual("Parameter description",
                parameterDescriptionValue,
                "Parameter description attribute did not cause description to be set correctly in the swagger metadata.");
        }

        [TestMethod, TestCategory("x-ms-summary"), TestCategory("description"), TestCategory("Parameter Attribute")]
        public void Parameter_NoAttributesSet_NoSwaggerCustomizationsApplied()
        {
            var parameterNode = Swagger.SelectToken(@"paths./test/x-ms-summary/no-attribute.post.parameters[?(@.name=='parameter')]");

            Assert.IsNotNull("Parameter node for no-attribute parameters test missing in the swagger metadata.");

            var descriptionNode = parameterNode.SelectToken(@"description");

            Assert.IsNull(descriptionNode, "Description was set for parameter without any attributes.");

            var summaryNode = parameterNode.SelectToken(@"x-ms-summary");

            Assert.IsNull(summaryNode, "Summary was set for parameter without any attributes.");

        }

        #endregion

        #region Property Attributes

        [TestMethod, TestCategory("x-ms-summary"), TestCategory("Property Attribute")]
        public void Property_FriendlyNameSet_FriendlyNameShownInSwagger()
        {
            var propertySummaryNode = Swagger.SelectToken(@"definitions.SummaryTestModel.properties.Property.x-ms-summary");

            var propertySummaryValue = propertySummaryNode.Value<string>();

            Assert.AreEqual("Property friendly name",
                propertySummaryValue,
                "Property friendly name attribute did not cause x-ms-summary to be set correctly in the swagger metadata.");

        }

        [TestMethod, TestCategory("description"), TestCategory("Property Attribute")]
        public void Property_DescriptionSet_DescriptionShownInDescription()
        {
            var propertyDescriptionNode = Swagger.SelectToken(@"definitions.SummaryTestModel.properties.Property.description");

            var propertyDescriptionValue = propertyDescriptionNode.Value<string>();

            Assert.AreEqual("Property description",
                propertyDescriptionValue,
                "Property description attribute did not cause description to be set correctly in the swagger metadata.");

        }

        [TestMethod, TestCategory("x-ms-summary"), TestCategory("description"), TestCategory("Property Attribute")]
        public void Property_NoAttributesSet_DescriptionNotShownInSwagger()
        {
            var propertyNode = Swagger.SelectToken(@"definitions.NoAttributeTestModel.properties.Property");

            Assert.IsNotNull("Property node for no-attribute property test missing in the swagger metadata.");

            var descriptionNode = propertyNode.SelectToken(@"description");

            Assert.IsNull(descriptionNode, "Description was set for property without any attributes.");

            var summaryNode = propertyNode.SelectToken(@"x-ms-summary");

            Assert.IsNull(summaryNode, "Summary was set for property without any attributes.");
        }

        #endregion
    }
}

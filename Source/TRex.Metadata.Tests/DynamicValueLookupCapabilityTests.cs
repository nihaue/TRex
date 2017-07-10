using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TRex.Test.Infrastructure;

namespace TRex.Metadata.Tests
{
    [TestClass]
    public class DynamicValueLookupCapabilityTests
    {
        public JToken Swagger = JToken.Parse(SwaggerResolver.Swagger);

        [TestMethod, TestCategory("x-ms-dynamic-values/capability"), TestCategory("Parameter Attribute")]
        public void Parameter_CapabilityParametersAsJson_ParametersObjectCorrectlyRepresentedInSwagger()
        {
            var dynamicValuesNode = Swagger
                .SelectToken(@"paths./test/x-ms-dynamic-values/capability/capability-json-parameters.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-values");

            var parametersNode = dynamicValuesNode.SelectToken("parameters");
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for ValueSource attribute.");
            var sampleParam1 = parametersNode.SelectToken("sampleParam1");
            Assert.IsNotNull(sampleParam1, "Parameters were not emitted with correct names for ValueSource attribute.");
            var sampleParam2 = parametersNode.SelectToken("sampleParam2");
            Assert.IsNotNull(sampleParam2, "Parameters were not emitted with correct names for ValueSource attribute.");
            var sampleParam3 = parametersNode.SelectToken("sampleParam3");
            Assert.IsNotNull(sampleParam2, "Parameters were not emitted with correct names for ValueSource attribute.");

            Assert.AreEqual("noAttributeParameter", sampleParam1.SelectToken("parameter").Value<string>(), "Parameter template not resolved correctly in ValueSource attribute");
            Assert.AreEqual("hardcoded-value", sampleParam2.Value<string>(), "Hard coded parameter not resolved correctly in ValueSource attribute");
            Assert.AreEqual("true", sampleParam3.Value<string>().ToLower(), "'true' parameter not resolved correctly in ValueSource attribute");
        }

        [TestMethod, TestCategory("x-ms-dynamic-values/capability"), TestCategory("Parameter Attribute")]
        public void Parameter_CapabilityNormalParameters_ParametersObjectCorrectlyRepresentedInSwagger()
        {
            var dynamicValuesNode = Swagger
                .SelectToken(@"paths./test/x-ms-dynamic-values/capability/capability-normal-parameters.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-values");
            
            var parametersNode = dynamicValuesNode.SelectToken("parameters");
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for ValueSource attribute.");
            var sampleParam1 = parametersNode.SelectToken("sampleParam1");
            Assert.IsNotNull(sampleParam1, "Parameters were not emitted with correct names for ValueSource attribute.");
            var sampleParam2 = parametersNode.SelectToken("sampleParam2");
            Assert.IsNotNull(sampleParam2, "Parameters were not emitted with correct names for ValueSource attribute.");
            var sampleParam3 = parametersNode.SelectToken("sampleParam3");
            Assert.IsNotNull(sampleParam2, "Parameters were not emitted with correct names for ValueSource attribute.");

            Assert.AreEqual("noAttributeParameter", sampleParam1.SelectToken("parameter").Value<string>(), "Parameter template not resolved correctly in ValueSource attribute");
            Assert.AreEqual("hardcoded-value", sampleParam2.Value<string>(), "Hard coded parameter not resolved correctly in ValueSource attribute");
            Assert.AreEqual("true", sampleParam3.Value<string>().ToLower(), "'true' parameter not resolved correctly in ValueSource attribute");
        }

        [TestMethod, TestCategory("x-ms-dynamic-values/capability"), TestCategory("Parameter Attribute")]
        public void Parameter_CapabilityNoParameterValue_ParameterValueIsStringEmptyInSwagger()
        {
            var dynamicValuesNode = Swagger
                .SelectToken(@"paths./test/x-ms-dynamic-values/capability/capability-no-parameter-value.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-values");

            var parametersNode = dynamicValuesNode.SelectToken("parameters");
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for ValueSource attribute.");
            var sampleParam1 = parametersNode.SelectToken("sampleParam1");
            Assert.IsNotNull(sampleParam1, "Parameters were not emitted with correct names for ValueSource attribute.");

            Assert.AreEqual("", sampleParam1, "Parameter template not resolved correctly in ValueSource attribute");
        }

        [TestMethod, TestCategory("x-ms-dynamic-values/capability"), TestCategory("Parameter Attribute")]
        public void Parameter_CapabilityNullParameters_ParameterIsStringEmptyInSwagger()
        {
            var dynamicValuesNode = Swagger
                .SelectToken(@"paths./test/x-ms-dynamic-values/capability/capability-null-parameters.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-values");

            var parametersNode = dynamicValuesNode.SelectToken("parameters");
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for ValueSource attribute.");

            Assert.IsFalse (parametersNode.HasValues, "Parameter template not resolved correctly in ValueSource attribute");
        }
    }
}

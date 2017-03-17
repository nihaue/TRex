using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using TRex.Test.Infrastructure;

namespace TRex.Metadata.Tests
{
    
    [TestClass]
    
    public class DynamicSchemaLookupTests
    {
        public JToken Swagger = JToken.Parse(SwaggerResolver.Swagger);
        
        [TestMethod, TestCategory("x-ms-dynamic-schema"), TestCategory("Parameter Attribute")]
        public void Parameter_SchemaLookupOperationParametersAsJson_ParametersObjectCorrectlyRepresentedInSwagger()
        {
            var dynamicSchemaNode = Swagger.SelectToken(@"definitions.DynamicSchemaJsonParameters.x-ms-dynamic-schema");

            var parametersNode = dynamicSchemaNode.SelectToken("parameters");
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for DynamicSchemaLookup attribute.");
            var sampleParam1 = parametersNode.SelectToken("sampleParam1");
            Assert.IsNotNull(sampleParam1, "Parameters were not emitted with correct names for DynamicSchemaLookup attribute.");
            var sampleParam2 = parametersNode.SelectToken("sampleParam2");
            Assert.IsNotNull(sampleParam2, "Parameters were not emitted with correct names for DynamicSchemaLookup attribute.");


            Assert.AreEqual("noAttributeParameter", sampleParam1.SelectToken("parameter").Value<string>(), "Parameter template not resolved correctly in DynamicSchemaLookup attribute");
            Assert.AreEqual("hardcoded-value", sampleParam2.Value<string>(), "Hard coded parameter not resolved correctly in DynamicSchemaLookup attribute");
            
        }
        
        [TestMethod, TestCategory("x-ms-dynamic-schema"), TestCategory("Parameter Attribute")]
        public void Parameter_SchemaLookupOperationNoParameters_NoParametersShowInSwagger()
        {

            var dynamicSchemaNode = Swagger.SelectToken(@"definitions.DynamicSchemaNoParamsModel.x-ms-dynamic-schema");
            var parametersNode = dynamicSchemaNode.SelectToken("parameters");

            Assert.IsFalse(parametersNode.HasValues,
                "Parameters found for lookup operation without parameters specified in DynamicSchemaLookup attribute.");
        }

        [TestMethod, TestCategory("x-ms-dynamic-schema"), TestCategory("Parameter Attribute")]
        public void Parameter_SchemaLookupOperationNullParamValue_EmptyStringValueShownInSwagger()
        {
            var dynamicSchemaNode = Swagger.SelectToken(@"definitions.DynamicSchemaNullParamModel.x-ms-dynamic-schema");
            var parametersNode = dynamicSchemaNode.SelectToken("parameters");
            
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for DynamicSchemaLookup attribute.");

            Assert.IsTrue(parametersNode.HasValues, "Parameters node emitted for DynamicSchemaLookup attribute does not have values.");

            var sampleParam1 = parametersNode.SelectToken("sampleParam1");
            Assert.IsNotNull(sampleParam1, "Parameters were not emitted with correct names for DynamicSchemaLookup attribute.");

            Assert.AreEqual(string.Empty, sampleParam1.Value<string>(), "Null parameter in parameter template was assigned non-null value in Swagger");

        }

        [TestMethod, TestCategory("x-ms-dynamic-schema"), TestCategory("Parameter Attribute")]
        public void Parameter_FullSchemaLookupInfo_FullInfoAppearsInSwagger()
        {
            var dynamicSchemaNode = Swagger.SelectToken(@"definitions.DynamicSchemaTestModel.x-ms-dynamic-schema");

            var operationIdNode = dynamicSchemaNode.SelectToken("operationId");
            Assert.IsNotNull(operationIdNode, "Operation Id was not emitted for DynamicSchemaLookup attribute.");
            Assert.AreEqual("FriendlyNameForOperation",
                operationIdNode.Value<string>(),
                "Operation Id emitted by DynamicSchemaLookup attribute contains incorrect value.");

            var parametersNode = dynamicSchemaNode.SelectToken("parameters");
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for DynamicSchemaLookup attribute.");
            var sampleParam1 = parametersNode.SelectToken("sampleParam1");
            Assert.IsNotNull(sampleParam1, "Parameters were not emitted with correct names for DynamicSchemaLookup attribute.");
            var sampleParam2 = parametersNode.SelectToken("sampleParam2");
            Assert.IsNotNull(sampleParam2, "Parameters were not emitted with correct names for DynamicSchemaLookup attribute.");

            Assert.AreEqual("noAttributeParameter", sampleParam1.SelectToken("parameter").Value<string>(), "Place holder parameter not expanded correctly in handling of DynamicSchemaLookup attribute");            
            Assert.AreEqual("hardcoded-value-here", sampleParam2.Value<string>(), "Hard coded parameter not resolved correctly in DynamicSchemaLookup attribute");
            

            var valuePathNode = dynamicSchemaNode.SelectToken("value-path");
            Assert.IsNotNull(valuePathNode, "Value path was not emitted for DynamicSchemaLookup attribute.");
            Assert.AreEqual("Id", valuePathNode.Value<string>(), "Value Path emitted by DynamicSchemaLookup attribute contains incorrect value.");

        }
        
    }
}

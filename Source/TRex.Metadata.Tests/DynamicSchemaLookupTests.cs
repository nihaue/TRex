using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using TRex.TestHelpers;

namespace TRex.Metadata.Tests
{
    
    [TestClass]
    public class DynamicSchemaLookupTests
    {
        public JToken Swagger = JToken.Parse(SwaggerResolver.Swagger);

        [TestMethod, TestCategory("x-ms-dynamic-schema"), TestCategory("Method Attribute")]
        public void ReturnTypeLookup_AttributeDefined_DynamicSchemaEmittedInSwagger()
        {
            var dynamicSchemaNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-schema/dynamic-schema-return.get.responses.200.x-ms-dynamic-schema");

            Assert.IsNotNull(dynamicSchemaNode, "Dynamic schema lookup metadata not emitted for operation");

            var operationIdNode = dynamicSchemaNode.SelectToken("operationId");

            Assert.IsNotNull(operationIdNode, "Operation Id was not emitted for DynamicSchemaLookup attribute.");
            Assert.AreEqual("FriendlyNameForOperation",
                operationIdNode.Value<string>(),
                "Operation Id emitted by DynamicSchemaLookup attribute contains incorrect value.");

            var parametersNode = dynamicSchemaNode.SelectToken("parameters");
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for DynamicSchemaLookup attribute.");
            var sampleParam1 = parametersNode.SelectToken("sampleParam1");
            Assert.IsNotNull(sampleParam1, "Parameters were not emitted with correct names for DynamicSchemaLookup attribute.");

            Assert.AreEqual("{sampleParam1}", sampleParam1.Value<string>(), "Parameter template not resolved correctly in DynamicSchemaLookup attribute");

            var valuePathNode = dynamicSchemaNode.SelectToken("value-path");
            Assert.IsNotNull(valuePathNode, "Value path was not emitted for DynamicSchemaLookup attribute.");
            Assert.AreEqual("schema", valuePathNode.Value<string>(), "Value Path emitted by DynamicSchemaLookup attribute contains incorrect value.");

        }

        [TestMethod, TestCategory("x-ms-dynamic-schema"), TestCategory("Parameter Attribute")]
        public void Parameter_SchemaLookupOperationFoundOnClass_OperationIdResolvedViaReflection()
        {

            var lookupOperationIdNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-schema/friendly-operation-as-source.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-schema.operationId");

            var lookupOperationIdNodeValue = lookupOperationIdNode.Value<string>();

            Assert.AreEqual("FriendlyNameForOperation",
                lookupOperationIdNodeValue,
                "Lookup operation id was not appropriately resolved from the method name.");
            
        }

        [TestMethod, TestCategory("x-ms-dynamic-schema"), TestCategory("Parameter Attribute")]
        public void Parameter_SchemaLookupOperationLacksFriendlyNameAttribute_UsedLiterallyAsOperationId()
        {

            var lookupOperationIdNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-schema/no-attribute-operation-as-source.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-schema.operationId");

            var lookupOperationIdNodeValue = lookupOperationIdNode.Value<string>();

            Assert.AreEqual("NoAttributeSource",
                lookupOperationIdNodeValue,
                "Lookup operation id was not appropriately resolved from the method name.");

        }

        [TestMethod, TestCategory("x-ms-dynamic-schema"), TestCategory("Parameter Attribute")]
        public void Parameter_SchemaLookupOperationParametersAsJson_ParametersObjectCorrectlyRepresentedInSwagger()
        {
            var dynamicSchemaNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-schema/literal-operationid-json-parameters.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-schema");

            var parametersNode = dynamicSchemaNode.SelectToken("parameters");
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for DynamicSchemaLookup attribute.");
            var sampleParam1 = parametersNode.SelectToken("sampleParam1");
            Assert.IsNotNull(sampleParam1, "Parameters were not emitted with correct names for DynamicSchemaLookup attribute.");
            var sampleParam2 = parametersNode.SelectToken("sampleParam2");
            Assert.IsNotNull(sampleParam2, "Parameters were not emitted with correct names for DynamicSchemaLookup attribute.");

            Assert.AreEqual("{noAttributeParameter}", sampleParam1.Value<string>(), "Parameter template not resolved correctly in DynamicSchemaLookup attribute");
            Assert.AreEqual("hardcoded-value", sampleParam2.Value<string>(), "Hard coded parameter not resolved correctly in DynamicSchemaLookup attribute");
            
        }

        [TestMethod, TestCategory("x-ms-dynamic-schema"), TestCategory("Parameter Attribute")]
        public void Parameter_SchemaLookupOperationNotFoundOnController_UsedLiterally()
        {
            var dynamicSchemaNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-schema/literal-operationid-no-parameters.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-schema");

            var operationId = dynamicSchemaNode.Value<string>("operationId");

            Assert.AreEqual("Some_Other_OperationId", operationId,
                "Operation Id emitted for DynamicSchemaLookup attribute was not the literal operation Id specified.");
        }

        [TestMethod, TestCategory("x-ms-dynamic-schema"), TestCategory("Parameter Attribute")]
        public void Parameter_SchemaLookupOperationNoParameters_NoParametersShowInSwagger()
        {

            var dynamicSchemaNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-schema/literal-operationid-no-parameters.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-schema");
            var parametersNode = dynamicSchemaNode.SelectToken("parameters");

            Assert.IsFalse(parametersNode.HasValues,
                "Parameters found for lookup operation without parameters specified in DynamicSchemaLookup attribute.");
        }

        [TestMethod, TestCategory("x-ms-dynamic-schema"), TestCategory("Parameter Attribute")]
        public void Parameter_SchemaLookupOperationNullParamValue_EmptyStringValueShownInSwagger()
        {
            var dynamicSchemaNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-schema/literal-operationid-null-parameter.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-schema");
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
            var dynamicSchemaNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-schema/friendly-operation-as-source.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-schema");

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

            Assert.AreEqual("{noAttributeParameter}", sampleParam1.Value<string>(), "Parameter template not resolved correctly in DynamicSchemaLookup attribute");
            Assert.AreEqual("hardcoded-value-here", sampleParam2.Value<string>(), "Hard coded parameter not resolved correctly in DynamicSchemaLookup attribute");
            
            var valuePathNode = dynamicSchemaNode.SelectToken("value-path");
            Assert.IsNotNull(valuePathNode, "Value path was not emitted for DynamicSchemaLookup attribute.");
            Assert.AreEqual("Id", valuePathNode.Value<string>(), "Value Path emitted by DynamicSchemaLookup attribute contains incorrect value.");

        }
        
    }
}

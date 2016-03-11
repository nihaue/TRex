using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TRex.Test.Infrastructure;

namespace TRex.Metadata.Tests
{


    //"/some/path/{parameter}": {
    //	"get": {
    //		"operationId": "OperationByParameterAndOtherParam1",
    //		"parameters": [
    //			{
    //				"name": "parameter",
    //				"x-ms-dynamic-values": {
    //					"operationId": "GetPossibleValues",
    //					"parameters": {
    //						    "sampleParam1": "{otherParam1}",
    //						    "sampleParam2": "hardcoded"
    //                  }
    //					"value-collection": "ItemList",
    //					"value-path": "Id",
    //					"value-title": "DisplayName"
    //				}

    //			}
    //		]

    //	}
    //}

    [TestClass]
    public class DynamicValueLookupTests
    {
        public JToken Swagger = JToken.Parse(SwaggerResolver.Swagger);

        [TestMethod, TestCategory("x-ms-dynamic-values"), TestCategory("Parameter Attribute")]
        public void Parameter_LookupOperationFoundOnClass_OperationIdResolvedViaReflection()
        {

            var lookupOperationIdNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-values/friendly-operation-as-source.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-values.operationId");

            var lookupOperationIdNodeValue = lookupOperationIdNode.Value<string>();

            Assert.AreEqual("FriendlyNameForOperation",
                lookupOperationIdNodeValue,
                "Lookup operation id was not appropriately resolved from the method name.");
            
        }

        [TestMethod, TestCategory("x-ms-dynamic-values"), TestCategory("Parameter Attribute")]
        public void Parameter_LookupOperationLacksFriendlyNameAttribute_UsedLiterallyAsOperationId()
        {

            var lookupOperationIdNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-values/no-attribute-operation-as-source.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-values.operationId");

            var lookupOperationIdNodeValue = lookupOperationIdNode.Value<string>();

            Assert.AreEqual("NoAttributeSource",
                lookupOperationIdNodeValue,
                "Lookup operation id was not appropriately resolved from the method name.");

        }

        [TestMethod, TestCategory("x-ms-dynamic-values"), TestCategory("Parameter Attribute")]
        public void Parameter_LookupOperationParametersAsJson_ParametersObjectCorrectlyRepresentedInSwagger()
        {
            var dynamicValuesNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-values/literal-operationid-json-parameters.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-values");

            var parametersNode = dynamicValuesNode.SelectToken("parameters");
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for ValueSource attribute.");
            var sampleParam1 = parametersNode.SelectToken("sampleParam1");
            Assert.IsNotNull(sampleParam1, "Parameters were not emitted with correct names for ValueSource attribute.");
            var sampleParam2 = parametersNode.SelectToken("sampleParam2");
            Assert.IsNotNull(sampleParam2, "Parameters were not emitted with correct names for ValueSource attribute.");

            Assert.AreEqual("{noAttributeParameter}", sampleParam1.Value<string>(), "Parameter template not resolved correctly in ValueSource attribute");
            Assert.AreEqual("hardcoded-value", sampleParam2.Value<string>(), "Hard coded parameter not resolved correctly in ValueSource attribute");
            
        }

        [TestMethod, TestCategory("x-ms-dynamic-values"), TestCategory("Parameter Attribute")]
        public void Parameter_LookupOperationNotFoundOnController_UsedLiterally()
        {
            var dynamicValuesNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-values/literal-operationid-no-parameters.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-values");

            var operationId = dynamicValuesNode.Value<string>("operationId");

            Assert.AreEqual("Some_Other_OperationId", operationId,
                "Operation Id emitted for ValueSource attribute was not the literal operation Id specified.");
        }

        [TestMethod, TestCategory("x-ms-dynamic-values"), TestCategory("Parameter Attribute")]
        public void Parameter_LookupOperationNoParameters_NoParametersShowInSwagger()
        {

            var dynamicValuesNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-values/literal-operationid-no-parameters.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-values");
            var parametersNode = dynamicValuesNode.SelectToken("parameters");

            Assert.IsFalse(parametersNode.HasValues,
                "Parameters found for lookup operation without parameters specified in ValueSource attribute.");
        }

        [TestMethod, TestCategory("x-ms-dynamic-values"), TestCategory("Parameter Attribute")]
        public void Parameter_LookupOperationNullParamValue_EmptyStringValueShownInSwagger()
        {
            var dynamicValuesNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-values/literal-operationid-null-parameter.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-values");
            var parametersNode = dynamicValuesNode.SelectToken("parameters");
            
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for ValueSource attribute.");

            Assert.IsTrue(parametersNode.HasValues, "Parameters node emitted for ValueSource attribute does not have values.");

            var sampleParam1 = parametersNode.SelectToken("sampleParam1");
            Assert.IsNotNull(sampleParam1, "Parameters were not emitted with correct names for ValueSource attribute.");

            Assert.AreEqual(string.Empty, sampleParam1.Value<string>(), "Null parameter in parameter template was assigned non-null value in Swagger");

        }

        [TestMethod, TestCategory("x-ms-dynamic-values"), TestCategory("Parameter Attribute")]
        public void Parameter_FullValueLookupInfo_FullInfoAppearsInSwagger()
        {
            var dynamicValuesNode = Swagger.SelectToken(@"paths./test/x-ms-dynamic-values/friendly-operation-as-source.get.parameters[?(@.name == 'lookupParameter')].x-ms-dynamic-values");

            var operationIdNode = dynamicValuesNode.SelectToken("operationId");
            Assert.IsNotNull(operationIdNode, "Operation Id was not emitted for ValueSource attribute.");
            Assert.AreEqual("FriendlyNameForOperation",
                operationIdNode.Value<string>(),
                "Operation Id emitted by ValueSource attribute contains incorrect value.");

            var parametersNode = dynamicValuesNode.SelectToken("parameters");
            Assert.IsNotNull(parametersNode, "Parameters were not emitted for ValueSource attribute.");
            var sampleParam1 = parametersNode.SelectToken("sampleParam1");
            Assert.IsNotNull(sampleParam1, "Parameters were not emitted with correct names for ValueSource attribute.");
            var sampleParam2 = parametersNode.SelectToken("sampleParam2");
            Assert.IsNotNull(sampleParam2, "Parameters were not emitted with correct names for ValueSource attribute.");

            Assert.AreEqual("{noAttributeParameter}", sampleParam1.Value<string>(), "Parameter template not resolved correctly in ValueSource attribute");
            Assert.AreEqual("hardcoded-value-here", sampleParam2.Value<string>(), "Hard coded parameter not resolved correctly in ValueSource attribute");
            
            var valueCollectionNode = dynamicValuesNode.SelectToken("value-collection");
            Assert.IsNotNull(valueCollectionNode, "Value Collection was not emitted for ValueSource attribute.");
            Assert.AreEqual("ItemList", valueCollectionNode.Value<string>(), "Value Collection emitted by ValueSource attribute contains incorrect value.");

            var valuePathNode = dynamicValuesNode.SelectToken("value-path");
            Assert.IsNotNull(valuePathNode, "Value path was not emitted for ValueSource attribute.");
            Assert.AreEqual("Id", valuePathNode.Value<string>(), "Value Path emitted by ValueSource attribute contains incorrect value.");

            var valueTitleNode = dynamicValuesNode.SelectToken("value-title");
            Assert.IsNotNull(valueTitleNode, "Value title was not emitted for ValueSource attribute.");
            Assert.AreEqual("DisplayName", valueTitleNode.Value<string>(), "Value Title emitted by ValueSource attribute contains incorrect value.");
        }
        
    }
}

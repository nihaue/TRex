using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TRex.Metadata.Models;
using TRex.Test.Infrastructure;

namespace TRex.Metadata.Tests
{
    [TestClass]
    public class FilePickerCapabilityTests
    {
        [TestMethod]
        public void Properties_NormalFilePickerCapabilityModel_FilePickerCapabilityObjectCorrectlyRepresentedInSwagger()
        {
            var capability = new FilePickerCapabilityModel(
                new FilePickerOperationModel("openOperationId", null),
                new FilePickerOperationModel(
                    "browseOperationId",
                    new Dictionary<string, string> {{"parameterName", "parameterValue"}}),
                "value-title value",
                "value-folder-property value",
                "value-media-property value"
            );

            var swagger = JToken.Parse(SwaggerResolver.GetSwagger(capability));

            var filePickerNode = swagger.SelectToken(@"x-ms-capabilities.file-picker");
            Assert.IsNotNull(filePickerNode);

            var openOperationNode = filePickerNode.SelectToken(@"open");
            Assert.IsNotNull(openOperationNode);
            var browseOperationNode = filePickerNode.SelectToken(@"browse");
            Assert.IsNotNull(browseOperationNode);
            var valueTitle = filePickerNode.SelectToken(@"value-title");
            Assert.IsNotNull(valueTitle);
            var valueFolderProperty = filePickerNode.SelectToken(@"value-folder-property");
            Assert.IsNotNull(valueFolderProperty);
            var valueMediaProperty = filePickerNode.SelectToken(@"value-media-property");
            Assert.IsNotNull(valueMediaProperty);

            var openOperationId = openOperationNode.SelectToken(@"operationId");
            Assert.IsNotNull(openOperationId);
            var openParameters = openOperationNode.SelectToken(@"parameters");
            Assert.IsNull(openParameters);
            var browseOperationId = browseOperationNode.SelectToken(@"operationId");
            Assert.IsNotNull(browseOperationId);
            var browseParameters = browseOperationNode.SelectToken(@"parameters");
            Assert.IsNotNull(browseParameters);
            var browseParameterName = browseParameters.SelectToken(@"parameterName");
            Assert.IsNotNull(browseParameterName);
            var browseParameterValue = browseParameterName.SelectToken(@"value-property");
            Assert.IsNotNull(browseParameterValue);

            Assert.AreEqual(openOperationId.Value<string>(), "openOperationId");
            Assert.AreEqual(browseOperationId.Value<string>(), "browseOperationId");
            Assert.AreEqual(browseParameterValue.Value<string>(), "parameterValue");

            Assert.AreEqual(valueTitle.Value<string>(), "value-title value");
            Assert.AreEqual(valueFolderProperty.Value<string>(), "value-folder-property value");
            Assert.AreEqual(valueMediaProperty.Value<string>(), "value-media-property value");
        }

        [TestMethod]
        public void PropertyValues_NullFilePickerOperationIds_EmptyOperationsInSwagger()
        {
            var capability = new FilePickerCapabilityModel(
                new FilePickerOperationModel(null, null),
                new FilePickerOperationModel(
                    null,
                    new Dictionary<string, string> {{"parameterName", "parameterValue"}}),
                "value-title value",
                "value-folder-property value",
                "value-media-property value"
            );

            JToken swagger = JToken.Parse(SwaggerResolver.GetSwagger(capability));

            var filePickerNode = swagger.SelectToken(@"x-ms-capabilities.file-picker");
            Assert.IsNotNull(filePickerNode);

            var openOperationNode = filePickerNode.SelectToken(@"open");
            Assert.IsNotNull(openOperationNode);
            var browseOperationNode = filePickerNode.SelectToken(@"browse");
            Assert.IsNotNull(browseOperationNode);

            Assert.IsFalse(openOperationNode.HasValues);
            Assert.IsFalse(browseOperationNode.HasValues);
        }

        //properties other than operations open and browse
        [TestMethod]
        public void PropertyValues_NullFilePickerProperties_DoesntGenerateThemInSwagger()
        {
            var capability = new FilePickerCapabilityModel(
                new FilePickerOperationModel("openOperationId", null),
                new FilePickerOperationModel(
                    "browseOperationId",
                    new Dictionary<string, string> {{"parameterName", "parameterValue"}}),
                null,
                null,
                null
            );

            JToken swagger = JToken.Parse(SwaggerResolver.GetSwagger(capability));

            var filePickerNode = swagger.SelectToken(@"x-ms-capabilities.file-picker");
            Assert.IsNotNull(filePickerNode);

            var valueTitle = filePickerNode.SelectToken(@"value-title");
            Assert.IsNull(valueTitle);
            var valueFolderProperty = filePickerNode.SelectToken(@"value-folder-property");
            Assert.IsNull(valueFolderProperty);
            var valueMediaProperty = filePickerNode.SelectToken(@"value-media-property");
            Assert.IsNull(valueMediaProperty);
        }
    }
}

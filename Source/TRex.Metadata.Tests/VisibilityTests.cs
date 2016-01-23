using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TRex.TestHelpers;

namespace TRex.Metadata.Tests
{
    [TestClass]
    public class VisibilityTests
    {
        public JToken Swagger = JToken.Parse(SwaggerResolver.Swagger);

        #region Property Attributes

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Property Attribute")]
        public void Property_AdvancedVisibility_VendorExtensionApplied()
        {
            var visibilityExtension = Swagger.SelectToken(@"definitions.VisibilityTestModel.properties.Advanced.x-ms-visibility");
            var visibilityValue = visibilityExtension.Value<string>();
            Assert.AreEqual("advanced", visibilityValue, "Advanced visibility not correctly applied through the x-ms-visibility vendor extension on property.");

        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Property Attribute")]
        public void Property_DefaultVisibility_NoVendorExtensionApplied()
        {
            var parameter = Swagger.SelectToken(@"definitions.VisibilityTestModel.properties.Default");
            Assert.IsNotNull(parameter, "Property for test of Default visibility of x-ms-visibility vendor extension was not found.");

            var visibilityExtension = parameter.SelectToken("x-ms-visibility");
            Assert.IsNull(visibilityExtension, "Default Visibility attribute should not have resulted in the x-ms-visibility vendor extension being applied on property.");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Property Attribute")]
        public void Property_ImportantVisibility_VendorExtensionApplied()
        {
            var visibilityExtension = Swagger.SelectToken(@"definitions.VisibilityTestModel.properties.Important.x-ms-visibility");
            var visibilityValue = visibilityExtension.Value<string>();
            Assert.AreEqual("important", visibilityValue, "Important visibility not correctly applied through the x-ms-visibility vendor extension on property.");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Property Attribute")]
        public void Property_InternalVisibility_VendorExtensionApplied()
        {
            var visibilityExtension = Swagger.SelectToken(@"definitions.VisibilityTestModel.properties.Internal.x-ms-visibility");
            var visibilityValue = visibilityExtension.Value<string>();
            Assert.AreEqual("internal", visibilityValue, "Internal visibility not correctly applied through the x-ms-visibility vendor extension on property.");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Property Attribute")]
        public void Property_NoAttribute_NoVendorExtensionApplied()
        {
            var parameter = Swagger.SelectToken(@"definitions.VisibilityTestModel.properties.NoAttribute");
            Assert.IsNotNull(parameter, "Property for test of Default visibility of x-ms-visibility vendor extension was not found.");

            var visibilityExtension = parameter.SelectToken("x-ms-visibility");
            Assert.IsNull(visibilityExtension, "Omitting Visibility attribute should not have resulted in the x-ms-visibility vendor extension being applied on property.");
        }

        #endregion

        #region Parameter Attributes

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Parameter Attribute")]
        public void Parameter_ImportantVisibility_VendorExtensionApplied()
        {
            var visibilityExtension = Swagger.SelectToken(@"paths./test/x-ms-visibility/parameters.get.parameters[?(@.name=='important')].x-ms-visibility");
            var visibilityValue = visibilityExtension.Value<string>();
            Assert.AreEqual("important", visibilityValue, "Important visibility not correctly applied through the x-ms-visibility vendor extension on argument.");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Parameter Attribute")]
        public void Parameter_DefaultVisibility_NoVendorExtensionApplied()
        {
            var parameter = Swagger.SelectToken(@"paths./test/x-ms-visibility/parameters.get.parameters[?(@.name=='default')]");
            Assert.IsNotNull(parameter, "Parameter for test of Default visibility of x-ms-visibility vendor extension was not found.");

            var visibilityExtension = parameter.SelectToken("x-ms-visibility");
            Assert.IsNull(visibilityExtension, "Default visibility should not have resulted in the x-ms-visibility vendor extension being applied on parameter.");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Parameter Attribute")]
        public void Parameter_NoAttribute_NoVendorExtensionApplied()
        {
            var parameter = Swagger.SelectToken(@"paths./test/x-ms-visibility/parameters.get.parameters[?(@.name=='noattribute')]");
            Assert.IsNotNull(parameter, "Parameter for test of Default visibility of x-ms-visibility vendor extension was not found.");

            var visibilityExtension = parameter.SelectToken("x-ms-visibility");
            Assert.IsNull(visibilityExtension, "Omitting an Visibility attribute should not have resulted in the x-ms-visibility vendor extension being applied on parameter.");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Parameter Attribute")]
        public void Parameter_AdvancedVisibility_VendorExtensionApplied()
        {
            var visibilityExtension = Swagger.SelectToken(@"paths./test/x-ms-visibility/parameters.get.parameters[?(@.name=='advanced')].x-ms-visibility");
            var visibilityValue = visibilityExtension.Value<string>();
            Assert.AreEqual("advanced", visibilityValue, "Advanced visibility not correctly applied through the x-ms-visibility vendor extension on argument.");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Parameter Attribute")]
        public void Parameter_InternalVisibility_VendorExtensionApplied()
        {
            var visibilityExtension = Swagger.SelectToken(@"paths./test/x-ms-visibility/parameters.get.parameters[?(@.name=='internal')].x-ms-visibility");
            var visibilityValue = visibilityExtension.Value<string>();
            Assert.AreEqual("internal", visibilityValue, "Internal visibility not correctly applied through the x-ms-visibility vendor extension on argument.");
        }

        #endregion

        #region Method Attributes

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Method Attribute")]
        public void Method_ImportantVisibility_VendorExtensionApplied()
        {
            var visibilityExtension = Swagger.SelectToken("paths./test/x-ms-visibility/important.get.x-ms-visibility");
            var visibilityValue = visibilityExtension.Value<string>();
            Assert.AreEqual("important", visibilityValue, "Important visibility not correctly applied through the x-ms-visibility vendor extension on method.");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Method Attribute")]
        public void Method_AdvancedVisibility_VendorExtensionApplied()
        {
            var visibilityExtension = Swagger.SelectToken("paths./test/x-ms-visibility/advanced.get.x-ms-visibility");
            var visibilityValue = visibilityExtension.Value<string>();
            Assert.AreEqual("advanced", visibilityValue, "Advanced visibility not correctly applied through the x-ms-visibility vendor extension on method.");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Method Attribute")]
        public void Method_InternalVisibility_VendorExtensionApplied()
        {
            var visibilityExtension = Swagger.SelectToken("paths./test/x-ms-visibility/internal.get.x-ms-visibility");
            var visibilityValue = visibilityExtension.Value<string>();
            Assert.AreEqual("internal", visibilityValue, "Internal visibility not correctly applied through the x-ms-visibility vendor extension on method.");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Method Attribute")]
        public void Method_DefaultVisibility_NoVendorExtensionApplied()
        {
            var route = Swagger.SelectToken("paths./test/x-ms-visibility/default.get");
            Assert.IsNotNull(route, "Route for test of Default visibility of x-ms-visibility vendor extension was not found.");

            var visibilityExtension = route.SelectToken("x-ms-visibility");
            Assert.IsNull(visibilityExtension, "Default visibility should not have resulted in the x-ms-visibility vendor extension being applied on method.");
        }

        [TestMethod, TestCategory("x-ms-visibility"), TestCategory("Method Attribute")]
        public void Method_NoAttribute_NoVendorExtensionApplied()
        {
            var route = Swagger.SelectToken("paths./test/x-ms-visibility/no-attribute.get");
            Assert.IsNotNull(route, "Route for test of Default visibility of x-ms-visibility vendor extension was not found.");

            var visibilityExtension = route.SelectToken("x-ms-visibility");
            Assert.IsNull(visibilityExtension, "Omitting an Visibility attribute should not have resulted in the x-ms-visibility vendor extension being applied on method.");
        }

        #endregion

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using TRex.Test.Infrastructure;

namespace TRex.Metadata.Tests
{
    [TestClass]
    public class TriggerTests
    {
        public JToken Swagger = JToken.Parse(SwaggerResolver.Swagger);

        [TestMethod, TestCategory("x-ms-notification-url"), TestCategory("Property Attribute")]
        public void Property_CallbackUrlAttributeSet_XNotificationUrlShownInSwagger()
        {
            var notificationUrlNode = Swagger.SelectToken(@"definitions.SubscriptionTestModel.properties.NotificationUrl.x-ms-notification-url");

            Assert.IsNotNull(notificationUrlNode, "Notification Url node was not found on property where CallbackUrl attribute was applied.");

            var notificationUrlValue = notificationUrlNode.Value<string>();

            Assert.AreEqual("true",
                notificationUrlValue,
                "Notification Url property was not true on property where CallbackUrl attribute was applied.");
        }

        [TestMethod, TestCategory("x-ms-notification-url"), TestCategory("Property Attribute")]
        public void Property_CallbackUrlAttributeNotSet_XNotificationUrlNotEmitted()
        {

            var notificationUrlNode = Swagger.SelectToken(@"definitions.SubscriptionTestModel.properties.SampleProperty.x-ms-notification-url");

            Assert.IsNull(notificationUrlNode, "Notification Url was found for property without CallbackUrl attribute applied");

        }

        [TestMethod, TestCategory("x-ms-notification-content"), TestCategory("Method Attribute")]
        public void Operation_CallbackTypeIsPrimitiveType_XNotificationContentSchemaUsesType()
        {

            var notificationContentNode = Swagger.SelectToken(@"paths./test/x-ms-notification-content/$subscriptions.x-ms-notification-content");

            Assert.IsNotNull(notificationContentNode, "x-ms-notification-content extension not emitted for simple content test path");

            Assert.IsNotNull(notificationContentNode["schema"], "schema node not found for x-ms-notification-content vendor extension on simple notification type test path");

            Assert.IsNotNull(notificationContentNode["description"], "description node not found for x-ms-notification-content vendor extension on simple notification type test path");

            var notificationType = notificationContentNode["schema"]["type"].Value<string>();
            var notificationDescription = notificationContentNode["description"].Value<string>();

            Assert.AreEqual("string", notificationType);
            Assert.AreEqual("String callback value", notificationDescription);
            
        }

        [TestMethod, TestCategory("x-ms-notification-content"), TestCategory("Method Attribute")]
        public void Operation_CallbackTypeIsComplexType_XNotificationContentSchemaUsesRef()
        {
            var notificationContentNode = Swagger.SelectToken(@"paths./test/x-ms-notification-content/complex/$subscriptions.x-ms-notification-content");

            Assert.IsNotNull(notificationContentNode, "x-ms-notification-content extension not emitted for complex content test path");

            Assert.IsNotNull(notificationContentNode["schema"], "schema node not found for x-ms-notification-content vendor extension on complex notification type test path");

            Assert.IsNotNull(notificationContentNode["description"], "description node not found for x-ms-notification-content vendor extension on complex notification type test path");


            var notificationType = notificationContentNode["schema"]["$ref"].Value<string>();
            var notificationDescription = notificationContentNode["description"].Value<string>();

            Assert.AreEqual("#/definitions/NotificationContentTestModel", notificationType);
            Assert.AreEqual("Complex callback value", notificationDescription);

        }

        [TestMethod, TestCategory("x-ms-notification-content"), TestCategory("Method Attribute")]
        public void Operation_CallbackTypeIsComplexType_SchemaDefinitionIsRegisteredInSwagger()
        {
            var schemaDefinitionNode = Swagger.SelectToken(@"definitions.NotificationContentTestModel");

            Assert.IsNotNull(schemaDefinitionNode, "Schema for NotificationContentTestModel not registered.");
            
        }

        [TestMethod, TestCategory("x-ms-notification-content"), TestCategory("Method Attribute"), TestCategory("Property Attribute")]
        public void Operation_CallbackTypeIsComplexTypeWithMetadataAttribute_SchemaDefinitionHasVendorExtensions()
        {
            
            var visibilityExtension = Swagger.SelectToken(@"definitions.NotificationContentTestModel.properties.SampleIntProperty.x-ms-visibility");
            var visibilityValue = visibilityExtension.Value<string>();
            Assert.AreEqual("advanced", visibilityValue, "Advanced visibility not correctly applied through the x-ms-visibility vendor extension on property.");

            var summaryExtension = Swagger.SelectToken(@"definitions.NotificationContentTestModel.properties.SampleIntProperty.x-ms-summary");
            var summaryValue  = summaryExtension.Value<string>();
            Assert.AreEqual("Sample Int Property", summaryValue, "Summary not correctly applied through the x-ms-summary vendor extension on property.");

            var descriptionNode = Swagger.SelectToken(@"definitions.NotificationContentTestModel.properties.SampleIntProperty.description");
            var descriptionValue = descriptionNode.Value<string>();
            Assert.AreEqual("Contains a sample int property", descriptionValue, "Description not correctly applied through the description in swagger on property.");

        }

        [TestMethod, TestCategory("x-ms-trigger"), TestCategory("Method Attribute")]
        public void Operation_NotATrigger_NoTriggerMetadataPresent()
        {
            var triggerNode = Swagger.SelectToken(@"paths./test/x-ms-trigger/regular-op.get.x-ms-trigger");

            Assert.IsNull(triggerNode, "Trigger metadata was emitted for operation lacking the TriggerAttribute");
        }

        [TestMethod, TestCategory("x-ms-trigger"), TestCategory("Method Attribute")]
        public void Operation_BatchTrigger_BatchTriggerMetadataPresent()
        {
            var triggerNode = Swagger.SelectToken(@"paths./test/x-ms-trigger/batched.get.x-ms-trigger");

            Assert.IsNotNull(triggerNode, "x-ms-trigger attribute was not emitted for batched trigger");

            Assert.AreEqual("batch", triggerNode.Value<string>(),
                "x-ms-trigger attribute was not correctly emitted for batch trigger");
            
        }

        [TestMethod, TestCategory("x-ms-trigger"), TestCategory("Method Attribute")]
        public void Operation_BatchedTrigger_200ResponseDescriptionHasCorrectType()
        {
            var respNode200 = Swagger.SelectToken(@"paths./test/x-ms-trigger/batched.get.responses.200");

            Assert.IsNotNull(respNode200, "Batched polling operation should have response description, but none was emitted in the generated swagger");

            Assert.AreEqual("Lucky numbers", respNode200.Value<string>("description"), "Batched polling operation has incorrect type for 200 response description");          
        }

        [TestMethod, TestCategory("x-ms-trigger"), TestCategory("Method Attribute")]
        public void Operation_BatchedTrigger_202ResponseDescriptionPresent()
        {
            var respNode202 = Swagger.SelectToken(@"paths./test/x-ms-trigger/batched.get.responses.202");

            Assert.IsNotNull(respNode202, "Batched polling operation should have response 202 description, but none was emitted in the generated swagger");

            Assert.AreEqual("Accepted", respNode202.Value<string>("description"), "Batched polling operation has incorrect type for 200 response description");
        }

        [TestMethod, TestCategory("x-ms-trigger"), TestCategory("Method Attribute")]
        public void Operation_SingleTriggerExplicit_SingleTriggerMetadatPresent()
        {
            var triggerNode = Swagger.SelectToken(@"paths./test/x-ms-trigger/single.get.x-ms-trigger");

            Assert.IsNotNull(triggerNode, "x-ms-trigger attribute was not emitted for explicitly defined single trigger");

            Assert.AreEqual("single", triggerNode.Value<string>(),
                "x-ms-trigger attribute was not correctly emitted for explicitly defined single trigger");
        }

        [TestMethod, TestCategory("x-ms-trigger"), TestCategory("Method Attribute")]
        public void Operation_SingleTriggerImplicit_SingleTriggerMetadataPresent()
        {

            var triggerNode = Swagger.SelectToken(@"paths./test/x-ms-trigger/single-implicit.get.x-ms-trigger");

            Assert.IsNotNull(triggerNode, "x-ms-trigger attribute was not emitted for implicitly defined single trigger");

            Assert.AreEqual("single", triggerNode.Value<string>(),
                "x-ms-trigger attribute was not correctly emitted for implicitly defined single trigger");
        }

    }
}

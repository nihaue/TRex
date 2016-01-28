using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TRex.TestHelpers;

namespace TRex.Metadata.Tests
{
    [TestClass]
    public class AsyncCallbackTests
    {
        public JToken Swagger = JToken.Parse(SwaggerResolver.Swagger);

        [TestMethod, TestCategory("x-ms-notification-url"), TestCategory("Property Attribute")]
        public void Property_CallbackUrlAttributeSet_XNotificationContentShownInSwagger()
        {
            var notificationUrlNode = Swagger.SelectToken(@"definitions.SubscriptionTestModel.properties.NotificationUrl.x-ms-notification-url");

            Assert.IsNotNull(notificationUrlNode, "Notification Url node was not found on property where CallbackUrl attribute was applied.");

            var notificationUrlValue = notificationUrlNode.Value<string>();

            Assert.AreEqual("true",
                notificationUrlValue,
                "Notification Url property was not true on property where CallbackUrl attribute was applied.");
        }

        [TestMethod, TestCategory("x-ms-notification-url"), TestCategory("Property Attribute")]
        public void Property_CallbackUrlAttributeNotSet_XNotificationContentNotEmitted()
        {

            var notificationUrlNode = Swagger.SelectToken(@"definitions.SubscriptionTestModel.properties.SampleProperty.x-ms-notification-url");

            Assert.IsNull(notificationUrlNode, "Notification Url was found for property without CallbackUrl attribute applied");

        }

    }
}

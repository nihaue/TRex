using System;
using TRex.Metadata;

namespace TRex.TestApiApp.Models
{
    public class SubscriptionTestModel
    {

        public string SampleProperty { get; set; }

        [CallbackUrl]
        public Uri NotificationUrl { get; set; }
        
    }
}

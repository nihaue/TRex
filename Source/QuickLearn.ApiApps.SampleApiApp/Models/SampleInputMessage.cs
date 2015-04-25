using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRex.Metadata;

namespace QuickLearn.ApiApps.SampleApiApp.Models
{
    public class SampleInputMessage
    {

        [Metadata("String Property", "A happy string input value")]
        public string StringProperty { get; set; }

        [Metadata(Visibility = VisibilityTypes.Advanced, FriendlyName = "Advanced String Property")]
        public string AdvancedStringProperty { get; set; }
       
    }
}

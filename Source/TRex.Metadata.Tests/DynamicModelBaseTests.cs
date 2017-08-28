using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRex.Test.DummyApi.Models;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using TRex.Metadata.Tests.TestDoubles;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TRex.Metadata.Tests
{
    [TestClass]
    public class DynamicModelBaseTests
    {
        
        [TestMethod]
        [TestCategory("DynamicModelBase")]
        public void DynamicModelBase_XmlConversion_SerializesCorrectly()
        {
            // This is to verify approach used in formatter for sample api,
            // to address issue brought up here:
            // https://github.com/nihaue/TRex/issues/8

            var testModel = new DynamicSchemaTestModel(new
            {
                Value1 = 1,
                Value2 = "Two",
                Value3 = new
                {
                    NestedValue1 = "Nested",
                    NestedValue2 = "Value"
                }
            });

            dynamic dynTestModel = testModel;
            var serializedJsonString = JsonConvert.SerializeObject(new
            {
                Root = dynTestModel
            });

            var xmlDoc = JsonConvert.DeserializeXmlNode(serializedJsonString);
            MemoryStream serializedXmlStream = new MemoryStream(Encoding.Default.GetBytes(xmlDoc.OuterXml));
            var outputDoc = XDocument.Load(serializedXmlStream);

            Assert.AreEqual(Convert.ToString(dynTestModel.Value1),
                                outputDoc.Root.Element("Value1").Value,
                                "Property serialization of root level member failed");
            Assert.AreEqual(Convert.ToString(dynTestModel.Value3.NestedValue1),
                                outputDoc.Root.Element("Value3").Element("NestedValue1").Value,
                                "Property serialization of nested member failed");

        }

        [TestMethod]
        [TestCategory("DynamicModelBase")]
        public void DynamicModelBase_FromDictionary_PropertiesReadable()
        {
            var myDictionary = new Dictionary<string, object>
            {
                { "prop1", "value1" },
                { "prop2", 2 }
            };

            var testModel = new DynamicSchemaTestModel(myDictionary);

            dynamic dynTestModel = testModel;

            Assert.AreEqual(Convert.ToString(myDictionary["prop1"]), Convert.ToString(dynTestModel.prop1), "Dictionary value not readable from dynamic test model with dictionary as source");
            Assert.AreEqual(Convert.ToInt32(myDictionary["prop2"]), Convert.ToInt32(dynTestModel.prop2), "Dictionary value not readable from dynamic test model with dictionary as source");

        }

        [TestMethod]
        [TestCategory("DynamicModelBase")]
        public void DynamicModelBase_HardcodedProperty_WorksAlongsideDynamicProperties()
        {
            // Set a property via source object in constructor
            var testModel = new DynamicModelHardcodedProp(new
            {
                SourceProperty = "valueForSourceProperty"
            });

            // Set a property dynamically (adding it to the existing property)
            dynamic dynTestModel = testModel;

            dynTestModel.DynamicProperty = "valueForDynamicProperty";
            dynTestModel.DynamicNullProperty = null;
            
            // Set a property that was hardcoded as a member of the DynamicModelHardcodedProp
            testModel.HardcodedProperty = "valueForHardcodedProperty";
            testModel.HardcodedNullProperty = null;

            // Serialization round-trip
            var json = JsonConvert.SerializeObject(testModel);
            JObject result = JObject.Parse(json);

            // Check for properties
            Assert.AreEqual("valueForSourceProperty", result.Value<string>("SourceProperty"), "Source property not readable in output from dynamic model");
            Assert.AreEqual("valueForHardcodedProperty", result.Value<string>("HardcodedProperty"), "Hardcoded property not readable in output from dynamic model");
            Assert.IsNull(result.Value<string>("HardcodedNullProperty"), "Hardcoded null property not readable in output from dynamic model");
            Assert.AreEqual("valueForDynamicProperty",result.Value<string>("DynamicProperty"), "Dynamic property not readable in output from dynamic model");
            Assert.IsNull(result.Value<string>("DynamicNullProperty"), "Dynamic null property not readable in output from dynamic model");
        }

    }

}

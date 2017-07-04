using System.Collections.Generic;
using Newtonsoft.Json;
using QuickLearn.ApiApps.Metadata;

namespace TRex.Metadata.Models
    {
    public class FilePickerOperationModel
        {
        [JsonProperty (PropertyName = Constants.OPERATION_ID)]
        public string OperationId { get; set; }

        [JsonProperty (PropertyName = Constants.PARAMETERS)]
        public Dictionary<string, FilePickerParameterValue> Parameters { get; set; }

        public FilePickerOperationModel (string operationId, Dictionary<string, string> parameters)
            {
            if (string.IsNullOrEmpty (operationId))
                {
                return;
                }
            OperationId = operationId;
            if (parameters == null)
                return;
            Parameters = new Dictionary<string, FilePickerParameterValue> ();
            foreach (var param in parameters)
                {
                Parameters.Add (param.Key, new FilePickerParameterValue (param.Value));
                }
            }
        }

    public class FilePickerParameterValue
        {
        [JsonProperty(PropertyName = Constants.VALUE_PROPERTY)]
        public string Value
            {
            get; set;
            }

        public FilePickerParameterValue (string value)
            {
            Value = value;
            }
        }
    }
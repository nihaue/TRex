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

        /// <summary>
        /// Initializes a new instance of file picker operation with given parameters
        /// </summary>
        /// <param name="operationId">Id of operation</param>
        /// <param name="parameters">Parameter name and value pair</param>
        public FilePickerOperationModel (string operationId, Dictionary<string, string> parameters)
            {
            //no operation - dont create parameters
            if (string.IsNullOrEmpty (operationId))
                return;
                
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
        [JsonProperty (PropertyName = Constants.VALUE_PROPERTY)]
        public string Value { get; set; }

        public FilePickerParameterValue (string value)
            {
            Value = value;
            }
        }
    }
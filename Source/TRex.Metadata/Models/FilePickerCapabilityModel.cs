using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickLearn.ApiApps.Metadata;

namespace TRex.Metadata.Models
    {
    public class FilePickerCapabilityModel
        {
        [JsonProperty (PropertyName = Constants.OPEN)]
        public FilePickerOperation Open { get; set; }
        
        [JsonProperty (PropertyName = Constants.BROWSE)]
        public FilePickerOperation Browse { get; set; }

        [JsonProperty (PropertyName = Constants.VALUE_TITLE)]
        public string ValueTitle { get; set; }

        [JsonProperty (PropertyName = Constants.VALUE_FOLDER_PROPERTY)]
        public string ValueFolderProperty { get; set; }

        [JsonProperty (PropertyName = Constants.VALUE_MEDIA_PROPERTY)]
        public string ValueMediaProperty { get; set; }

        /// <summary>
        /// Initializes a new instance of FilePickerCapability with information supplied
        /// </summary>
        /// <param name="open">Operation which will be called when you initially when you start
        /// picking folder/file in MS Flow UI (when you press the folder icon)</param>
        /// <param name="browse">Operation will be called when you browse through folders in 
        /// MS Flow UI</param>
        /// <param name="valueTitle">Value from either of operations (Open or Browse) output
        /// which will be shown in MS Flow UI</param>
        /// <param name="valueFolderProperty">Value from either of operations (Open or Browse) output
        /// used to determine whether the output is a folder or a file</param>
        /// <param name="valueMediaProperty">Value from either of operations (Open or Browse) output
        /// used to determine file type (.zip, .txt, etc.)</param>
        public FilePickerCapabilityModel 
        (
        FilePickerOperation open,
        FilePickerOperation browse,
        string valueTitle,
        string valueFolderProperty,
        string valueMediaProperty
        )
            {
            Open = open;
            //remove parameters from open, they need to be in x-ms-dynamic-values
            //should i even leave it as a FilePickerOperation?
            Open.Parameters = null;
            Browse = browse;
            ValueTitle = valueTitle;
            ValueFolderProperty = valueFolderProperty;
            ValueMediaProperty = valueMediaProperty;
            }
        }

    public class FilePickerOperation
        {
        [JsonProperty(PropertyName = Constants.OPERATION_ID)]
        public string OperationId { get; set; }
        
        [JsonProperty(PropertyName = Constants.PARAMETERS)]
        public Dictionary<string, FilePickerParameterValue> Parameters { get; set; }

        public FilePickerOperation (string operationId, Dictionary<string, string> parameters)
            {
            OperationId = operationId;
            if (parameters == null)
                return;
            Parameters = new Dictionary<string, FilePickerParameterValue> ();
            foreach (var param in parameters)
                {
                Parameters.Add (param.Key, new FilePickerParameterValue(param.Value));
                }
            }
        }
    
    public class FilePickerParameterValue
        {
        [JsonProperty(PropertyName = Constants.VALUE_PROPERTY)]
        public string Value { get; set; }

        public FilePickerParameterValue (string value)
            {
            Value = value;
            }
        }
    }
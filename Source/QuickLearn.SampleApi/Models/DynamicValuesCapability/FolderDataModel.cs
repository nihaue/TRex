using System.Runtime.Serialization;

namespace QuickLearn.SampleApi.Models.DynamicValuesCapability
    {
    public class FolderDataModel
        {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public bool IsFolder { get; set; }

        public string ParentName { get; set; }

        public FolderDataModel (string id, string name, string path, bool isFolder, string parentName)
            {
            Id = id;
            Name = name;
            Path = path;
            IsFolder = isFolder;
            ParentName = parentName;
            }

        public FolderDataModel ()
            {
            }
        }
    }
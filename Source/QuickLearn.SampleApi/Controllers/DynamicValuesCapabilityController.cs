using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickLearn.SampleApi.Models.DynamicValuesCapability;
using Swashbuckle.Swagger.Annotations;
using TRex.Metadata;

namespace QuickLearn.SampleApi.Controllers
{
    public class DynamicValuesCapabilityController : ApiController
    {
        [HttpGet, Route("api/folders")]
        [Metadata("GetRootFolders")]
        public IHttpActionResult GetRootFolders ()
            {
            return Ok (FolderTestData.testDataList.Where (x => x.ParentName == ""));
            }

        [HttpGet, Route("api/folder/{folderName}")]
        [Metadata("GetChildFolders")]
        public IHttpActionResult GetChildFolders (
            string folderName)
            {
            return Ok (FolderTestData.testDataList.Where (x => x.ParentName == folderName));
            }

        [HttpGet, Route("api/folderHierarchy/{test}")]
        [Metadata("TestHierarchy")]
        public IHttpActionResult TestHierarchy (
            [DynamicValueLookupCapability("file-picker", "isFolder=true", "Id", "Path")]
            string test
            )
            {
            return Ok ();
            }
    }

    public static class FolderTestData
        {
        //Hierarchy of these folders look like this
        /*
        name1
            name4
                name10
                name11
                name12
            name5
                name13
                name14
            name6
        name2
            name7
                name15
                name16
            name8
            name9
        name3
        */
        public static List<FolderDataModel> testDataList = new List<FolderDataModel>
            {
            new FolderDataModel ("1", "name1", "/name1", true, ""),
            new FolderDataModel ("2", "name2", "/name2", true, ""),
            new FolderDataModel ("3", "name3", "/name3", true, ""),
            new FolderDataModel ("4", "name4", "/name1/name4", true, "name1"),
            new FolderDataModel ("5", "name5", "/name1/name5", true, "name1"),
            new FolderDataModel ("6", "name6", "/name1/name6", true, "name1"),
            new FolderDataModel ("7", "name7", "/name2/name7", true, "name2"),
            new FolderDataModel ("8", "name8", "/name2/name8", true, "name2"),
            new FolderDataModel ("9", "name9", "/name2/name9", true, "name2"),
            new FolderDataModel ("10", "name10", "/name1/name4/name10", true, "name4"),
            new FolderDataModel ("11", "name11", "/name1/name4/name11", true, "name4"),
            new FolderDataModel ("12", "name12", "/name1/name4/name12", true, "name4"),
            new FolderDataModel ("13", "name13", "/name1/name5/name13", true, "name5"),
            new FolderDataModel ("14", "name14", "/name1/name5/name14", true, "name5"),
            new FolderDataModel ("15", "name15", "/name2/name7/name15", true, "name7"),
            new FolderDataModel ("16", "name16", "/name2/name7/name16", true, "name7"),
            };
        }
}

using Newtonsoft.Json.Linq;
using System;
using System.Security.Cryptography;
using System.Text;
using TRex.Metadata;

namespace QuickLearn.SampleApi.Models
{
    public class PageQueryResult
    {
        public PageQueryResult()
        {
            Results = new string[] { };
        }

        [Metadata("Matching Elements", "Elements on page that match the selector configured on the polling trigger")]
        public string[] Results { get; set; }

        public string GetHash()
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(JObject.FromObject(this).ToString())));
        }
    }
}
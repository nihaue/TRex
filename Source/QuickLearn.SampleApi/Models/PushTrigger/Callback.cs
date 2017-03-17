using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuickLearn.SampleApi.Models
{
    public class Callback<TConfiguration>
    {
        public TConfiguration Configuration { get; set; }

        public Uri UriWithCredentials { get; set; }

        public Uri RawUri
        {
            get
            {
                return new Uri(UriWithCredentials.GetComponents(UriComponents.SchemeAndServer | UriComponents.PathAndQuery, UriFormat.Unescaped));
            }
        }

        public string UserName
        {
            get
            {
                return string.IsNullOrWhiteSpace(UriWithCredentials.UserInfo) ? null :
                                UriWithCredentials.UserInfo.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
        }

        public string Password
        {
            get
            {
                return string.IsNullOrWhiteSpace(UriWithCredentials.UserInfo) ? null :
                                UriWithCredentials.UserInfo.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)
                                .Skip(1)
                                .Aggregate((s, p) => string.Concat(s, p));
            }
        }

        private string authorizationHeader
        {
            get
            {
                return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password)
                                ? string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", UserName, Password))))
                                : string.Empty;
            }
        }

        public Callback() { }

        public Callback(Uri uriWithCredentials)
            : this()
        {
            this.UriWithCredentials = uriWithCredentials;
        }

        public Callback(Uri uriWithCredentials, TConfiguration configuration)
            : this(uriWithCredentials)
        {
            Configuration = configuration;
        }

        public async Task<HttpResponseMessage> InvokeAsync<TOutput>(TOutput triggerOutput)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(authorizationHeader))
                client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

            JObject outputs = new JObject(
                new JProperty("outputs",
                    new JObject(
                        new JProperty("body",
                                      JToken.FromObject(triggerOutput)
                        )
                    )
                )
            );

            return await client.PostAsJsonAsync<JObject>(RawUri, outputs);

        }
    }
}
using QuickLearn.SampleApi.Formatters;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace QuickLearn.SampleApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            
            // For use with those actions that return types that derive from DynamicModelBase
            GlobalConfiguration.Configuration.Formatters.Add(new DynamicXmlFormatter());
            // For use with all other actions
            GlobalConfiguration.Configuration.Formatters.Add(new XmlMediaTypeFormatter());
        }
    }
}

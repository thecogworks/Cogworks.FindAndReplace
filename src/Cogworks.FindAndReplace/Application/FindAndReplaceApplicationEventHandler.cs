using System.Web.Configuration;
using Umbraco.Core;

namespace Cogworks.FindAndReplace.Application
{
    public class FindAndReplaceApplicationEventHandler : ApplicationEventHandler
    {

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)

        {
            bool enableFullTextSearch;
            bool.TryParse(WebConfigurationManager.AppSettings["FindAndReplace:EnableFullTextSearch"], out enableFullTextSearch);

            FindAndReplaceContext.Instance.EnableFullTextSearch = enableFullTextSearch;

        }

    }
}

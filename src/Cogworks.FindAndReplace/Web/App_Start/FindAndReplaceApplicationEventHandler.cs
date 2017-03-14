using Umbraco.Core;
using Umbraco.Web.Trees;

namespace Cogworks.FindAndReplace.Web.App_Start
{
    public class FindAndReplaceApplicationEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication,
            ApplicationContext application)
        {
            TreeControllerBase.MenuRendering += FindAndReplace_MenuRendering;
        }

        private void FindAndReplace_MenuRendering(global::Umbraco.Web.Trees.TreeControllerBase sender, global::Umbraco.Web.Trees.MenuRenderingEventArgs e)
        {
            switch (sender.TreeAlias)
            {
                case "content":
                    var menuItem = new global::Umbraco.Web.Models.Trees.MenuItem("findAndReplace", "Find and Replace");
                    menuItem.AdditionalData.Add("actionView", "/App_Plugins/FindAndReplace/Views/findandreplace.html");
                    menuItem.AdditionalData.Add("contentId", e.NodeId);
                    menuItem.Icon = "axis-rotation-2";
                    e.Menu.Items.Insert(e.Menu.Items.Count, menuItem);
                    break;
            }
        }
    }
}
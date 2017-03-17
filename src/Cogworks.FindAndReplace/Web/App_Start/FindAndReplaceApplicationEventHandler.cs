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

        private static void FindAndReplace_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        {
            if (sender.TreeAlias != "content") return;

            var menuItem = new Umbraco.Web.Models.Trees.MenuItem("findAndReplace", "Find and Replace");
            menuItem.AdditionalData.Add("actionView", "/App_Plugins/FindAndReplace/Views/findandreplace.html");
            menuItem.AdditionalData.Add("contentId", e.NodeId);
            menuItem.Icon = "axis-rotation-2";
            menuItem.SeperatorBefore = true;

            e.Menu.Items.Insert(e.Menu.Items.Count, menuItem);
        }
    }
}
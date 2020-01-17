using Umbraco.Core.Composing;
using Umbraco.Web.Trees;

namespace Cogworks.FindAndReplace.Web.Components
{
    public class FindAndReplaceComponent : IComponent
    {
        public void Initialize()
        {
            TreeControllerBase.MenuRendering += FindAndReplace_MenuRendering;
        }

        private static void FindAndReplace_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        {
            if (sender.TreeAlias != "content") return;

            var menuItem = new Umbraco.Web.Models.Trees.MenuItem("findAndReplace", "Find and Replace")
            {
                AdditionalData =
                {
                    { "actionView", "/App_Plugins/FindAndReplace/Views/findandreplace.html" },
                    { "contentId", e.NodeId }
                },
                Icon = "axis-rotation-2",
                SeparatorBefore = true
            };

            e.Menu.Items.Insert(e.Menu.Items.Count, menuItem);
        }

        public void Terminate()
        {
            TreeControllerBase.MenuRendering -= FindAndReplace_MenuRendering;
        }
    }
}
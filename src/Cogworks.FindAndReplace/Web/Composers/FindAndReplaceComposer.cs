using Cogworks.FindAndReplace.Web.Components;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Cogworks.FindAndReplace.Web.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class FindAndReplaceComposer : IComposer
    {
        public void Compose(Composition composition)
            => composition.Components().Append<FindAndReplaceComponent>();
    }
}
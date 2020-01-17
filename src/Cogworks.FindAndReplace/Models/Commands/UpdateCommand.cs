namespace Cogworks.FindAndReplace.Models.Commands
{
    public class UpdateCommand
    {
        public int VersionId { get; set; }

        public string PropertyAlias { get; set; }

        public string Value { get; set; }
    }
}
namespace Cogworks.FindAndReplace.Models.Dtos.RequestDtos
{
    public class ContentDataModel
    {
        public string NodeName { get; set; }

        public int VersionId { get; set; }

        public string PropertyAlias { get; set; }

        public string VarcharValue { get; set; }

        public string TextValue { get; set; }

        public string PropertyName { get; set; }

        public string Value => !string.IsNullOrWhiteSpace(VarcharValue)
            ? VarcharValue
            : TextValue;
    }
}

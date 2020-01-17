using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Cogworks.FindAndReplace.Models.Dtos.DatabaseDtos
{
    [TableName(TableName)]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class PropertyDataDto
    {
        public const string TableName = Constants.DatabaseSchema.Tables.PropertyData;
        public const int VarcharLength = 512;

        [Column("id")]
        [PrimaryKeyColumn]
        public int Id { get; set; }

        [Column("versionId")]
        [ForeignKey(typeof(ContentVersionDto))]
        [Index(IndexTypes.UniqueNonClustered, Name = "IX_" + TableName + "_VersionId", ForColumns = "versionId,propertyTypeId,languageId,segment")]
        public int VersionId { get; set; }

        [Column("propertyTypeId")]
        [ForeignKey(typeof(PropertyTypeDto))]
        [Index(IndexTypes.NonClustered, Name = "IX_" + TableName + "_PropertyTypeId")]
        public int PropertyTypeId { get; set; }

        [Column("languageId")]
        [ForeignKey(typeof(LanguageDto))]
        [Index(IndexTypes.NonClustered, Name = "IX_" + TableName + "_LanguageId")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public int? LanguageId { get; set; }


        [Column("varcharValue")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [Length(VarcharLength)]
        public string VarcharValue { get; set; }

        [Column("textValue")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string TextValue { get; set; }

        [ResultColumn]
        [Reference(ReferenceType.OneToOne, ColumnName = "PropertyTypeId")]
        public PropertyTypeDto PropertyTypeDto { get; set; }

        [Ignore]
        public object Value
        {
            get
            {
                if (!string.IsNullOrEmpty(VarcharValue))
                    return VarcharValue;

                if (!string.IsNullOrEmpty(TextValue))
                    return TextValue;

                return null;
            }
        }

        public PropertyDataDto Clone(int versionId)
        {
            return new PropertyDataDto
            {
                VersionId = versionId,
                PropertyTypeId = PropertyTypeId,
                LanguageId = LanguageId,
                VarcharValue = VarcharValue,
                TextValue = TextValue,
                PropertyTypeDto = PropertyTypeDto
            };
        }

        protected bool Equals(PropertyDataDto other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object other)
        {
            return
                !ReferenceEquals(null, other) // other is not null
                && (ReferenceEquals(this, other) // and either ref-equals, or same id
                    || other is PropertyDataDto pdata && pdata.Id == Id);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Id;
        }
    }
}
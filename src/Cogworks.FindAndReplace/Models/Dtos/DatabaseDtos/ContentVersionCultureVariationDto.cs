using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Cogworks.FindAndReplace.Models.Dtos.DatabaseDtos
{
    [TableName(TableName)]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class ContentVersionCultureVariationDto
    {
        public const string TableName = Constants.DatabaseSchema.Tables.ContentVersionCultureVariation;

        [Column("id")]
        [PrimaryKeyColumn]
        public int Id { get; set; }

        [Column("versionId")]
        [ForeignKey(typeof(ContentVersionDto))]
        [Index(IndexTypes.UniqueNonClustered, Name = "IX_" + TableName + "_VersionId", ForColumns = "versionId,languageId")]
        public int VersionId { get; set; }

        [Column("languageId")]
        [ForeignKey(typeof(LanguageDto))]
        [Index(IndexTypes.NonClustered, Name = "IX_" + TableName + "_LanguageId")]
        public int LanguageId { get; set; }


        [Column("name")]
        public string Name { get; set; }
    }
}
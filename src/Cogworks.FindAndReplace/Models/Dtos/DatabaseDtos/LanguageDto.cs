using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Cogworks.FindAndReplace.Models.Dtos.DatabaseDtos
{
    [TableName(TableName)]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class LanguageDto
    {
        public const string TableName = Constants.DatabaseSchema.Tables.Language;

        /// <summary>
        /// Gets or sets the identifier of the language.
        /// </summary>
        [Column("id")]
        [PrimaryKeyColumn(IdentitySeed = 2)]
        public short Id { get; set; }

        /// <summary>
        /// Gets or sets the ISO code of the language.
        /// </summary>
        [Column("languageISOCode")]
        [Index(IndexTypes.UniqueNonClustered)]
        [NullSetting(NullSetting = NullSettings.Null)]
        [Length(14)]
        public string IsoCode { get; set; }
    }
}
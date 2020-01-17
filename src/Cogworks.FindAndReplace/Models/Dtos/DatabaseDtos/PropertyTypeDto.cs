using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Cogworks.FindAndReplace.Models.Dtos.DatabaseDtos
{
    [TableName(TableName)]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class PropertyTypeDto
    {
        public const string TableName = Constants.DatabaseSchema.Tables.PropertyType;

        [Column("id")]
        [PrimaryKeyColumn(IdentitySeed = 50)]
        public int Id { get; set; }

        [Index(IndexTypes.NonClustered, Name = "IX_cmsPropertyTypeAlias")]
        [Column("Alias")]
        public string Alias { get; set; }

        [Column("Name")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Name { get; set; }

        [Column("Description")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [Length(2000)]
        public string Description { get; set; }
    }
}
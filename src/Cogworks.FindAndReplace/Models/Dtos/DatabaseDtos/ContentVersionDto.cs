using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Cogworks.FindAndReplace.Models.Dtos.DatabaseDtos
{
    [TableName(TableName)]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class ContentVersionDto
    {
        public const string TableName = Constants.DatabaseSchema.Tables.ContentVersion;

        [Column("id")]
        [PrimaryKeyColumn]
        public int Id { get; set; }

        [Column("nodeId")]
        [ForeignKey(typeof(ContentDto))]
        public int NodeId { get; set; }

        [Column("current")]
        public bool Current { get; set; }

        [Column("text")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Text { get; set; }

        [ResultColumn]
        [Reference(ReferenceType.OneToOne, ColumnName = "NodeId", ReferenceMemberName = "NodeId")]
        public ContentDto ContentDto { get; set; }
    }
}
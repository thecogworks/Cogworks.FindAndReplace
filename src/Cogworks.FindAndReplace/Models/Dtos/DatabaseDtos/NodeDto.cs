using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Cogworks.FindAndReplace.Models.Dtos.DatabaseDtos
{
    [TableName(TableName)]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class NodeDto
    {
        public const string TableName = Constants.DatabaseSchema.Tables.Node;
        public const int NodeIdSeed = 1060;

        [Column("id")]
        [PrimaryKeyColumn(IdentitySeed = NodeIdSeed)]
        public int NodeId { get; set; }

        [Column("parentId")]
        [ForeignKey(typeof(NodeDto))]
        [Index(IndexTypes.NonClustered, Name = "IX_" + TableName + "_ParentId")]
        public int ParentId { get; set; }

        [Column("path")]
        [Length(150)]
        [Index(IndexTypes.NonClustered, Name = "IX_" + TableName + "_Path")]
        public string Path { get; set; }

        [Column("text")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Text { get; set; }
    }
}
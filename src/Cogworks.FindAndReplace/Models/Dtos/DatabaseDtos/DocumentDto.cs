using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Cogworks.FindAndReplace.Models.Dtos.DatabaseDtos
{
    [TableName(TableName)]
    [PrimaryKey("nodeId", AutoIncrement = false)]
    [ExplicitColumns]
    public class DocumentDto
    {
        private const string TableName = Constants.DatabaseSchema.Tables.Document;

        [Column("nodeId")]
        [PrimaryKeyColumn(AutoIncrement = false)]
        [ForeignKey(typeof(ContentDto))]
        public int NodeId { get; set; }

        [Column("published")]
        [Index(IndexTypes.NonClustered, Name = "IX_" + TableName + "_Published")]
        public bool Published { get; set; }

        [ResultColumn]
        [Reference(ReferenceType.OneToOne, ReferenceMemberName = "NodeId")]
        public ContentDto ContentDto { get; set; }

        [ResultColumn]
        [Reference(ReferenceType.OneToOne)]
        public DocumentVersionDto DocumentVersionDto { get; set; }

        [ResultColumn]
        [Reference(ReferenceType.OneToOne)]
        public DocumentVersionDto PublishedVersionDto { get; set; }
    }
}
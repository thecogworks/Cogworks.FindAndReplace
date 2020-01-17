using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Cogworks.FindAndReplace.Models.Dtos.DatabaseDtos
{
    [TableName(TableName)]
    [PrimaryKey("id", AutoIncrement = false)]
    [ExplicitColumns]
    public class DocumentVersionDto
    {
        private const string TableName = Constants.DatabaseSchema.Tables.DocumentVersion;

        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = false)]
        [ForeignKey(typeof(ContentVersionDto))]
        public int Id { get; set; }

        [Column("published")]
        public bool Published { get; set; }

        [ResultColumn]
        [Reference(ReferenceType.OneToOne)]
        public ContentVersionDto ContentVersionDto { get; set; }
    }
}
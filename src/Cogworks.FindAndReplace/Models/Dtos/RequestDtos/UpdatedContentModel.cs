namespace Cogworks.FindAndReplace.Models.Dtos.RequestDtos
{
    public class UpdatedContentModel
    {
        public int PreviousVersionId { get; set; }

        public int CurrentVersionId { get; set; }

        public bool Succeeded { get; set; }
    }
}
namespace backend.Models
{
    public class Film
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? PersonalReview { get; set; }
        public required int Rating { get; set; }
        public Guid FolderId { get; set; }

    }
}
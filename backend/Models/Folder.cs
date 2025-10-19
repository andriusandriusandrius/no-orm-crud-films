namespace backend.Models
{
    public class Folder
    {
        public Guid Id { get; set; }
        public required string Name;
        public string? Description;

        public required Guid UserId { get; set; }
    }
}
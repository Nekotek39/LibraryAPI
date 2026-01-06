 namespace LibraryAPI.DTOs
{
    public class UpdateBookDTO
    {
        public string? title { get; set; }
        public int? year { get; set; }
        public int? authorId { get; set; }
    }
}

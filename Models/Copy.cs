namespace LibraryAPI.Models
{
    public class Copy
    {
        public int id { get; set; }
        public int bookId { get; set; }
        public virtual Book book { get; set; }
    }
}
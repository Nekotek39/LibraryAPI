using LibraryAPI.Models;

namespace LibraryAPI.DTOs
{
    public class BookDTO
    {
        public string title { get; set; }
        public int year { get; set; }
        public int authorId { get; set; }

        public BookDTO() { }
        public BookDTO(Book book) => (title, year, authorId) = (book.title, book.year, book.authorId);
    }
}
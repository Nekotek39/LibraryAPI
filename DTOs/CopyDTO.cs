using LibraryAPI.Models;

namespace LibraryAPI.DTOs
{
    public class CopyDTO
    {
        public int id { get; set; }
        public int bookId { get; set; }

        public CopyDTO() { }
        public CopyDTO(Copy copy) => (id, bookId) = (copy.id, copy.bookId);
    }
}
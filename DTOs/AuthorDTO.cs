using LibraryAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class AuthorDTO
    {
        public int id { get; set; }
        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }

        public AuthorDTO() { }
        public AuthorDTO(Author author) => (id, first_name, last_name) = (author.id, author.first_name, author.last_name);
    }
}
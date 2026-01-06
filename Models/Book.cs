using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace LibraryAPI.Models
{
    public class Book
    {
        public int? id { get; set; }
        [Required]
        public string title { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Year must be a non-negative integer")]
        public int year { get; set; }
        public int authorId { get; set; }
        public Author? author { get; set; }
        [JsonIgnore]
        public List<Copy> copies { get; set; } = new();
    }
}

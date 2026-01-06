using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI.Models
{
    public class Author
    {
        public int id { get; set; }
        [Required]
        public string first_name {  get; set; }
        [Required]
        public string last_name { get; set; }
        [JsonIgnore]
        public ICollection<Book> books { get; set; } = new List<Book>();
    }
}

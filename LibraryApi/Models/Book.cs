using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]

        public string Status { get; set; }
        

    }
}

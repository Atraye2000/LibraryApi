using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String UserName { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
    }
}

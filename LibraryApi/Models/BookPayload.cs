namespace LibraryApi.Models
{
    public class BookPayload
    {
        private List<Book> list;

        public List<Book> Books { get; set; }

        public int Count { get; set; }

        public BookPayload(List<Book> Books)
        {
            this.Books = Books;
            // this.Count = Count;
        }
    }
}

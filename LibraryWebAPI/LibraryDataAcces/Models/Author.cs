using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDataAcces.Models
{
    public class Author
    {
        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}

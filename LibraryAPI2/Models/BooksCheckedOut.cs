using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI2.Models
{
    public class BooksCheckedOut
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime DueDate { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI2.Models
{
    public class BookWithStatus
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public DateTime Genre { get; set; }
        public bool isAvailable { get; set; }
    }
}

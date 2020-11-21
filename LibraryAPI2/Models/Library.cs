using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI2.Models
{
    public partial class Library
    {
        public Library()
        {
            BookLibraryAssociation = new HashSet<BookLibraryAssociation>();
        }

        [Key]
        public int LibraryId { get; set; }
        public int? LocationId { get; set; }

        [InverseProperty("Library")]
        public virtual ICollection<BookLibraryAssociation> BookLibraryAssociation { get; set; }
    }
}

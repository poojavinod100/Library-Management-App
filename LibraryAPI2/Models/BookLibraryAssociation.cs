using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI2.Models
{
    public partial class BookLibraryAssociation
    {
        public BookLibraryAssociation()
        {
            UserBookAssociation = new HashSet<UserBookAssociation>();
        }

        [Key]
        public int BookLibraryAssociationId { get; set; }
        public int BookId { get; set; }
        public int LibraryId { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsCheckedOut { get; set; }

        [ForeignKey(nameof(BookId))]
        [InverseProperty(nameof(Books.BookLibraryAssociation))]
        public virtual Books Book { get; set; }
        [ForeignKey(nameof(LibraryId))]
        [InverseProperty("BookLibraryAssociation")]
        public virtual Library Library { get; set; }
        [InverseProperty("BookLibraryAssociation")]
        public virtual ICollection<UserBookAssociation> UserBookAssociation { get; set; }
    }
}

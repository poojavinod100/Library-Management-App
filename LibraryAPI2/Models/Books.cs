using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI2.Models
{
    public partial class Books
    {
        public Books()
        {
            BookLibraryAssociation = new HashSet<BookLibraryAssociation>();
        }

        [Key]
        [Required(ErrorMessage = "BookId is required")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage="Author is required")]
        [MaxLength(100)]
        [RegularExpression("^[a-zA-Z0-9 ]*$")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [Column(TypeName = "datetime")]
        public DateTime Genre { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }

        [InverseProperty("Book")]
        public virtual ICollection<BookLibraryAssociation> BookLibraryAssociation { get; set; }
    }
}

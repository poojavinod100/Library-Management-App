using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI2.Models
{
    public partial class UserBookAssociation
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookLibraryAssociationId { get; set; }
        [Column(TypeName = "date")]
        public DateTime DueDate { get; set; }

        [ForeignKey(nameof(BookLibraryAssociationId))]
        [InverseProperty("UserBookAssociation")]
        public virtual BookLibraryAssociation BookLibraryAssociation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserBookAssociation")]
        public virtual User User { get; set; }
    }
}

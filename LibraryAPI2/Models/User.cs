using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI2.Models
{
    public partial class User
    {
        public User()
        {
            UserBookAssociation = new HashSet<UserBookAssociation>();
        }

        [Key]
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int? LocationId { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<UserBookAssociation> UserBookAssociation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI2.Models
{
    public partial class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
    }
}

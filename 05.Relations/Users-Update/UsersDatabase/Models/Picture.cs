﻿
namespace UsersDatabase.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Picture
    {
        public Picture()
        {
            Albums = new HashSet<Album>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string Caption { get; set; }

        public string Path { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
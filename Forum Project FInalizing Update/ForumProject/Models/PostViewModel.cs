using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ForumProject.Models
{
    public class PostViewModel
    {
        public int PostId { get; set; }

        [Required]
        [StringLength(69)]
        public string Topic { get; set; }

        [Required]
        [StringLength(2500)]
        public string Content { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
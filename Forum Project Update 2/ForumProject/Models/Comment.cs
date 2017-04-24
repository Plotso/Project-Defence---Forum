using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ForumProject.Models
{
    public class Comment
    {
        public Comment()
        {
            this.DateCreated = DateTime.Now;
        }
        
        [Key]
        public int CommentId { get; set; }

       
        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public  virtual Post Post { get; set; }


        [Required]
        [StringLength(2500)]
        public string Content { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
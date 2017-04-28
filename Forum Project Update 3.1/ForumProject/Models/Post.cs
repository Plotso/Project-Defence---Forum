using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ForumProject.Models
{
    public class Post
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.DateCreated = DateTime.Now;
        }

        [Key]
        public int PostId { get; set; }

        [Required]
        [StringLength(69)]
        public string Topic { get; set; }

        [Required]
        [StringLength(2500)]
        public string Content { get; set; }
        
        //[StringLength(2500)]
       
        public  virtual ICollection<Comment> Comments { get; set; }

        

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsAuthor(string authorId)
        {
            return this.AuthorId == authorId;
        }
    }
}
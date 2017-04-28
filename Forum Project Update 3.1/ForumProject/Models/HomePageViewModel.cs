using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForumProject.Models
{
    public class HomePageViewModel
    {
        public List<Post> RecentPosts { get; set; }

        public List<Post> LastCommentedPosts { get; set; }
    }
}
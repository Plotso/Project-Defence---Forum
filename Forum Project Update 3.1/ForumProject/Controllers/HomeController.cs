using ForumProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumProject.Controllers
{
    public class HomeController : Controller
    {
        private ForumDbContext db = new ForumDbContext();
        public ActionResult Index()
        {
            var viewModel = new HomePageViewModel();

            var posts = db.Posts.Include(p => p.Author).Include(p => p.Comments).ToList();

            //Adding recent posts to view model
            var recentPosts = posts.OrderByDescending(p => p.DateCreated).Take(3).ToList();
            viewModel.RecentPosts = recentPosts;

            //Adding last commented posts in view model
            var comments = db.Comments.OrderByDescending(c => c.DateCreated).ToList();
            var lastCommentedPosts = new List<Post>();
            foreach (var comment in comments)
            {
                var post = posts.Find(p => p.PostId == comment.PostId);

                if (!lastCommentedPosts.Contains(post))
                {
                    lastCommentedPosts.Add(post);
                }

                if (lastCommentedPosts.Count > 2)
                {
                    break;
                }
            }

            viewModel.LastCommentedPosts = lastCommentedPosts;


            return View(viewModel);
        }

        public ActionResult About()
        {
            return View();
        }


    }
}
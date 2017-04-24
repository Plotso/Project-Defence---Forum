using ForumProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumProject.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
            return RedirectToAction("AllPosts");
        }

        public ActionResult AllPosts()
        {
            using (var db = new ForumDbContext())
            {
                var posts = db.Posts
                    .Include(p => p.Author)
                    .ToList();

                return View(posts);
            }
        }

        public ActionResult ViewPost (int? id)
        {
            var db = new ForumDbContext();


                var post = db.Posts
                    .Where(p => p.PostId == id)
                    .FirstOrDefault();

                if (post == null)
                {
                    return HttpNotFound();
                }

                return View(post);
            

        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ForumDbContext())
                {
                    var authorId = User.Identity.GetUserId();
                    post.AuthorId = authorId;

                    db.Posts.Add(post);
                    db.SaveChanges();

                   
                    return RedirectToAction("Index");
                }

            }

            return View(post);
        }

        public ActionResult ViewAllComments()
        {
            using (var db = new ForumDbContext())
            {
                var comments = db.Posts
                    .Include(c => c.Author)
                    .ToList();
                return View(comments);
            }

        }

        [Authorize]
        [HttpGet]
        public ActionResult Comment(int id)
        {
            Comment comment = new Comment
            {
                PostId = id 
            };
            return View(comment);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Comment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                var db = new ForumDbContext();

                Comment dbComment = new Comment
                {
                    PostId = comment.PostId,
                    Content = comment.Content,
                    AuthorId = User.Identity.GetUserId()
                };
                
                    db.Comments.Add(dbComment);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                
            }

            return View(comment);
        }

        public bool IsAuthorized(Post post)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var isAuthor = post.IsAuthor(this.User.Identity.GetUserId());

            return isAdmin || isAuthor;
        }
    }
}
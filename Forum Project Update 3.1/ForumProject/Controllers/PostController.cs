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

                    return RedirectToAction("ViewPost/" + comment.PostId);
                
            }

            return View(comment);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            using (var db = new ForumDbContext())
            {
                var post = db.Posts
                    .Where(p => p.PostId == id)
                    .First();

                if (post == null || !IsAuthorized(post))
                {
                    return HttpNotFound();
                }

                return View(post);
            }
        }

        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int? id)
        {
            using (var db = new ForumDbContext())
            {
                var post = db.Posts
                    .Where(p => p.PostId == id)
                    .First();

                if (post == null || !IsAuthorized(post))
                {
                    return HttpNotFound();
                }

                db.Posts.Remove(post);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var db = new ForumDbContext())
            {
                var post = db.Posts
                    .Find(id);

                if (post == null || !IsAuthorized(post))
                {
                    return HttpNotFound();
                }

                var articleViewModel = new PostViewModel
                {
                    PostId = post.PostId,
                    Topic = post.Topic,
                    Content = post.Content,
                    AuthorId = post.AuthorId,
                    DateCreated = post.DateCreated
                };

                return View(articleViewModel);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ForumDbContext())
                {
                    var post = db.Posts
                    .Find(model.PostId);

                    if (post == null || !IsAuthorized(post))
                    {
                        return HttpNotFound();
                    }

                    post.Topic = model.Topic;
                    post.Content = model.Content;

                    //db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("ViewPost", new { id = model.PostId });
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteComment(int? id)
        {
            using (var db = new ForumDbContext())
            {
                var comment = db.Comments
                    .Where(p => p.CommentId == id)
                    .First();

                if (comment == null || !IsAuthorizedComment(comment))
                {
                    return HttpNotFound();
                }

                return View(comment);
            }
        }

        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDeleteComment(int? id)
        {
            using (var db = new ForumDbContext())
            {
                var comment = db.Comments
                    .Where(p => p.CommentId == id)
                    .First();

                if (comment == null || !IsAuthorizedComment(comment))
                {
                    return HttpNotFound();
                }

                db.Comments.Remove(comment);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditComment(int id)
        {
            using (var db = new ForumDbContext())
            {
                var comment = db.Comments
                    .Find(id);

                if (comment == null || !IsAuthorizedComment(comment))
                {
                    return HttpNotFound();
                }

                var articleViewModel = new CommentViewModel
                {
                    CommentId = comment.CommentId,
                    PostId = comment.PostId,
                    Content = comment.Content,
                    AuthorId = comment.AuthorId,
                    DateCreated = comment.DateCreated
                };

                return View(articleViewModel);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditComment(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ForumDbContext())
                {
                    var comment = db.Comments
                    .Find(model.PostId);

                    if (comment == null || !IsAuthorizedComment(comment))
                    {
                        return HttpNotFound();
                    }

                    
                    comment.Content = model.Content;

                    //db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("ViewPost/" + comment.PostId + "EditComment" + comment.CommentId);
                }

                
            }

            return View(model);
        }



        public bool IsAuthorized(Post post)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var isAuthor = post.IsAuthor(this.User.Identity.GetUserId());

            return isAdmin || isAuthor;
        }

        public bool IsAuthorizedComment(Comment comment)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var isAuthor = comment.IsAuthor(this.User.Identity.GetUserId());

            return isAdmin || isAuthor;
        }
    }
}
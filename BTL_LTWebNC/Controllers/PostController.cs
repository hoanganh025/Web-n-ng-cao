using BTL_LTWebNC.Data;
using Microsoft.AspNetCore.Mvc;
using BTL_LTWebNC.Models.EF;
using BTL_LTWebNC.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BTL_LTWebNC.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public IActionResult SearchPost(string searchString)
        {
            var posts = from p in _dbContext.DbPost select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                posts = _dbContext.DbPost
                        .Include(u => u.User)
                        .Include(c => c.ListComment)
                        .Include(c => c.ListLike)
                        .Include(d => d.ListPostGallery)
                        .Where(p => p.Title.Contains(searchString));
            }

            return View("IndexPost", posts.ToList());
        }

        public IActionResult IndexPost()
        {
            var listPost = _dbContext.DbPost
                .Include(u => u.User)
                .Include(c => c.ListComment)
                .Include(c => c.ListLike)
                .Include(d => d.ListPostGallery)
                .ToList();

            if(listPost == null)
            {
                return View("Error");
            }
            return View(listPost);
        }

        public IActionResult Details(int? id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (id == null || _dbContext.DbPost == null)
            {
                return View("Error");
            }
            var post = _dbContext.DbPost.
                Include(u => u.User)
                .Include(a => a.ListLike).ThenInclude(b => b.User)
                .Include(c => c.ListComment).ThenInclude(d => d.User)
                .Include(e => e.ListPostGallery)
                .FirstOrDefault(p => p.PostID == id);

            if(post == null)
            {
                return View("Error");
            }

            //dem so like
            post.LikeCount = post.ListLike?.Count ?? 0;
            //check xem da like chua?
            post.UserHasLiked = post.ListLike?.Any(l => l.UserID == userId) ?? false;

            return View(post);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(Post post)
        {
            if(post != null)
            {
                //luu 1 anh
                if(post.CoverPhoto != null)
                {
                    string folderPath = "Upload/Image/";
                    post.Urlimg = UploadImgPath(folderPath, post.CoverPhoto);
                }

                //luu nhieu anh
                if (post.FormFileCollection != null)
                {
                    string folderPath = "Upload/GalleryImage/";

                    post.ListPostGallery = new List<PostGallery>();

                    foreach (var file in post.FormFileCollection)
                    {
                        var gallery = new PostGallery()
                        {
                            FileNameImg = file.FileName,
                            URLImgGallery = UploadImgPath(folderPath, file),
                            PostID = post.PostID
                        };
                        post.ListPostGallery.Add(gallery);
                    }
                }

                var newPost = new Post()
                {
                    PostID = post.PostID,
                    Title = post.Title,
                    ContentPost = post.ContentPost,
                    Urlimg = post.Urlimg,
                    UserID = post.UserID,
                    CreateOn = DateTime.Now,
                    UpdateOn = DateTime.Now,
                    User = post.User,
                    ListComment = post.ListComment,
                    ListLike = post.ListLike,
                    ListPostGallery = post.ListPostGallery
                };

                _dbContext.DbPost.Add(newPost);
                _dbContext.SaveChanges();

                return RedirectToAction("IndexPost");
            }
            return View(post);
        }

        public string UploadImgPath (string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            string severFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            file.CopyTo(new FileStream(severFolder, FileMode.Create));

            return "/" + folderPath;
        }

        [HttpPost]
        public IActionResult AddComment(Comment comment)
        {
            if(comment == null)
            {
                return View("Error");
            }

            //them thoi gian tao
            comment.CreateOn = DateTime.Now;

            //nhan 1 comment thi add vao sql
            _dbContext.DbComment.Add(comment);
            _dbContext.SaveChanges();

            var post = _dbContext.DbPost
                .Include(u => u.User)
                .Include(a => a.ListLike).ThenInclude(b => b.User)
                .Include(c => c.ListComment).ThenInclude(d => d.User)
                .Include(e => e.ListPostGallery)
                .FirstOrDefault(p => p.PostID == comment.PostID);

            return View("Details", post);
        }

        [HttpPost]
        public IActionResult ToggleLike(int postId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var existingLike = _dbContext.DbLike.FirstOrDefault(l => l.PostID == postId && l.UserID == userId);

            if (existingLike == null)
            {
                // Thêm 1 like
                var like = new Like 
                { 
                    PostID = postId, 
                    UserID = (int)userId 
                };
                _dbContext.DbLike.Add(like);
            }
            else
            {
                // Xóa like
                _dbContext.DbLike.Remove(existingLike);
            }

            _dbContext.SaveChanges();

            // Trả về số lượng like cập nhật
            var likeCount = _dbContext.DbLike.Count(l => l.PostID == postId);
            return Json(new { likeCount });
        }


        //Admin///////////////////////////
        public IActionResult IndexAdminPost()
        {
            var listPost = _dbContext.DbPost
                .Include(u => u.User)
                .Include(c => c.ListComment)
                .Include(c => c.ListLike).ToList();

            return View(listPost);
        }

        [HttpGet]
        public IActionResult EditPost(int id)
        {
            var editPost = _dbContext.DbPost.FirstOrDefault(p => p.PostID == id);
            if (editPost == null)
            {
                return NotFound();
            }

            return View(editPost);
        }

        [HttpPost]
        public IActionResult EditPost(Post post)
        {
            if(post == null)
            {
                return NotFound();
            }

            //luu 1 anh
            if (post.CoverPhoto != null)
            {
                string folderPath = "Upload/Image/";
                post.Urlimg = UploadImgPath(folderPath, post.CoverPhoto);
            }

            //luu nhieu anh
            if (post.FormFileCollection != null)
            {
                string folderPath = "Upload/GalleryImage/";

                post.ListPostGallery = new List<PostGallery>();

                foreach (var file in post.FormFileCollection)
                {
                    var gallery = new PostGallery()
                    {
                        FileNameImg = file.FileName,
                        URLImgGallery = UploadImgPath(folderPath, file),
                        PostID = post.PostID
                    };
                    post.ListPostGallery.Add(gallery);
                }
            }

            //xoa listpostgallery cu
            var deleteImg = _dbContext.DbPostGallery.Where(a => a.PostID == post.PostID).ToList();
            if (deleteImg.Any())
            {
                _dbContext.DbPostGallery.RemoveRange(deleteImg);
                _dbContext.SaveChanges();
            }

            var editPost = _dbContext.DbPost.Find(post.PostID);
            if (editPost == null)
            {
                return NotFound();
            }

            editPost.PostID = post.PostID;
            editPost.Title = post.Title;
            editPost.ContentPost = post.ContentPost;
            editPost.Urlimg = post.Urlimg;
            editPost.UserID = post.UserID;
            editPost.ListPostGallery = post.ListPostGallery;

            _dbContext.SaveChanges();

            return RedirectToAction("IndexAdminPost");
        }

        [HttpGet]
        public IActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletePost = _dbContext.DbPost.Find(id);
            if (deletePost == null)
            {
                return NotFound();
            }

            return View(deletePost);
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var deletePost = _dbContext.DbPost.Find(id);
            var deletelikes = _dbContext.DbLike.Where(l => l.PostID == id).ToList();
            var deletecomments = _dbContext.DbComment.Where(a => a.PostID == id).ToList();
            var deleteImg = _dbContext.DbPostGallery.Where(b => b.PostID == id).ToList();

            if (deletelikes.Any())
            {
                _dbContext.DbLike.RemoveRange(deletelikes);
                _dbContext.SaveChanges();
            }

            if (deletecomments.Any())
            {
                _dbContext.DbComment.RemoveRange(deletecomments);
                _dbContext.SaveChanges();
            }

            if (deleteImg.Any())
            {
                _dbContext.DbPostGallery.RemoveRange(deleteImg);
                _dbContext.SaveChanges();
            }

            if (deletePost != null)
            {
                _dbContext.DbPost.Remove(deletePost);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("IndexAdminPost");
        }
        /////////////////////////
    }
}

using BTL_LTWebNC.Models.EF;
using Microsoft.EntityFrameworkCore;
using System;

namespace BTL_LTWebNC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Role> DbRole { get; set; }
        public DbSet<User> DbUser { get; set; }
        public DbSet<Post> DbPost { get; set; }
        public DbSet<Comment> DbComment { get; set; }
        public DbSet<Like> DbLike { get; set; }
        public DbSet<PostGallery> DbPostGallery { get; set; }
    }
}

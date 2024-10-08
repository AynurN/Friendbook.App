using Friendbook.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Data.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<ProfileImage> ProfileImages { get; set; }
        public DbSet<DirectMessage> DirectMessages { get; set; }
        public DbSet<PostLike> PostLikes{ get; set; }
        public DbSet<Notification> Notifications{ get; set; }
    }
}

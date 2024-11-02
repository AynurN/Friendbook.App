using Friendbook.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friendship>()
                .HasKey(f => new { f.AppUserId, f.FriendId }); 

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.AppUser)
                .WithMany(u => u.Friendships)
                .HasForeignKey(f => f.AppUserId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DirectMessage>()
          .HasOne(dm => dm.Sender)
       .WithMany()  
       .HasForeignKey(dm => dm.SenderId)
       .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<DirectMessage>()
                .HasOne(dm => dm.Receiver)
                .WithMany()  
                .HasForeignKey(dm => dm.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<PostLike>()
                .HasKey(pl => new { pl.AppUserId, pl.PostId }); 

            modelBuilder.Entity<PostLike>()
                .HasOne(pl => pl.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(pl => pl.PostId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<PostLike>()
                .HasOne(pl => pl.AppUser)
                .WithMany() 
                .HasForeignKey(pl => pl.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);


        }
      
      

    }
}

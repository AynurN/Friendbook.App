using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Core.Entities
{
    public class Post :BaseClass
    {
        public string Content { get; set; }

        //relational
        public ICollection<PostImage> PostImages { get; set; }
        public  AppUser User { get; set; }             
        public  ICollection<Comment> Comments { get; set; } 
        public  ICollection<PostLike> Likes { get; set; }


    }
}

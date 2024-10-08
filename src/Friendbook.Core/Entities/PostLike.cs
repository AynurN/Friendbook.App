using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Core.Entities
{
    public class PostLike :BaseClass
    {
        //relational
        public string AppUserId { get; set; }
        public int PostId { get; set; }
        public AppUser AppUser { get; set; }
        public Post Post { get; set; }
    }
}

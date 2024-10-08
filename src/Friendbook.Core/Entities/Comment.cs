using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Core.Entities
{
    public class Comment :BaseClass
    {
        public string Content { get; set; }
        //relational
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}

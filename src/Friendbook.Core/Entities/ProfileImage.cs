using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Core.Entities
{
    public class ProfileImage :BaseClass
    {
        public string ImageURL { get; set; }

        //relational
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}

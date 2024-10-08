using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Core.Entities
{
    public class Notification :BaseClass
    {
        public string Message { get; set; }
        public bool IsRead { get; set; }

        //relational
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}

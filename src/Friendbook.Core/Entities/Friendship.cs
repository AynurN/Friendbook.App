using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Core.Entities
{
    public class Friendship : BaseClass
    {
        public  bool IsAccepted { get; set; }

        //relational
        public string AppUserId { get; set; }
        public string FriendId {  get; set; }
        public  AppUser AppUser { get; set; }            
        public  AppUser Friend { get; set; }
    }
}

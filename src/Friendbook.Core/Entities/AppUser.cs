﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Core.Entities
{
    public class AppUser :IdentityUser
    {
        public string UserName { get; set; }

        //relational
        public int ProfileImageId { get; set; }
        public ProfileImage ProfileImage { get; set; }
        public ICollection<Post> Posts { get; set; }                  
        public  ICollection<Friendship> Friendships { get; set; } 
        public  ICollection<Notification> Notifications { get; set; } 
    }
}


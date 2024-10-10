using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Core.Entities
{
    public class DirectMessage :BaseClass
    {
        public string Content { get; set; }                 
        public DateTime SentAt { get; set; }                
        public string SenderId { get; set; }                   
        public string ReceiverId { get; set; }                 

        //relational
        public  AppUser Sender { get; set; }            
        public  AppUser Receiver { get; set; }          
    }
}

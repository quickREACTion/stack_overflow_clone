using System;
using System.Collections.Generic;

namespace CoderHelp.Models
{
    public class Comments
    {
        public int commentsId { get; set; }
        public string Comment { get; set; }
        public int usersId { get; set; }
        public int messagesId { get; set; }
        public Messages Message { get; set; }
        public Users User { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
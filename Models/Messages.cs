using System;
using System.Collections.Generic;

namespace CoderHelp.Models {
    public class Messages {

        public int messagesId { get; set; }

        public string Message { get; set; }

        public string Description { get; set;}
        public string CodeSnippit { get; set; }
        public string Bug { get; set; }
        public int usersId { get; set; }
        public Users User { get; set; }

        public List<Comments> Comments { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public Messages() {
            Comments = new List<Comments>();
        }
    }
}
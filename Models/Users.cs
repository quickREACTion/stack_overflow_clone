using System;
using System.Collections.Generic;

namespace CoderHelp.Models {
    public class Users {

        public int usersId { get; set; }

        public string Username { get; set; }

        public string Email { get; set;}
        public string Password { get; set; }
        public string Employment { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Bio { get; set; }
        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public List<Messages> Messages { get; set; }

        public List<Comments> Comments { get; set; }

        public Users() {
            Messages = new List<Messages>();
            Comments = new List<Comments>();
        }
    }

}
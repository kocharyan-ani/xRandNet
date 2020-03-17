using System;
using System.Collections.Generic;

namespace WebApi.Database.Models {
    public partial class Users {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
    }
}
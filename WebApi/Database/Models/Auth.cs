using System;
using System.Collections.Generic;

namespace WebApi.Database.Models {
    public partial class Auth {
        public string JwtSecretKey { get; set; }
    }
}
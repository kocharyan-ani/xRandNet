﻿using System;
using System.Collections.Generic;

namespace WebApi.Database.Models {
    public partial class App {
        public int Id { get; set; }
        public string Version { get; set; }
        public int FileId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ReleaseNotes { get; set; }

        public virtual Files File { get; set; }
    }
}
﻿using Microsoft.EntityFrameworkCore;
using WebApi.Database.Models;

namespace WebApi.Database.Context {
    public partial class xrandnetContext : DbContext {
        public xrandnetContext() {
        }

        public xrandnetContext(DbContextOptions<xrandnetContext> options)
            : base(options) {
        }

        public virtual DbSet<App> App { get; set; }
        public virtual DbSet<AppFile> AppFile { get; set; }
        public virtual DbSet<Bug> Bug { get; set; }
        public virtual DbSet<Info> Info { get; set; }
        public virtual DbSet<Link> Link { get; set; }
        public virtual DbSet<ManualFile> ManualFile { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
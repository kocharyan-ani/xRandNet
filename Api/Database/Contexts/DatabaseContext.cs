using Api.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Database.Context {
    public partial class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext {
        public DatabaseContext() {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) {
        }

        public virtual DbSet<App> Apps { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<AppFile> AppFiles { get; set; }
        public virtual DbSet<Bug> Bugs { get; set; }
        public virtual DbSet<Info> Info { get; set; }
        public virtual DbSet<Link> Links { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<ManualFile> ManualFile { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
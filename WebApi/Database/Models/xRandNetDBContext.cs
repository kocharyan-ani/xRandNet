using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApi.Database.Models {
    public partial class xRandNetDBContext : DbContext {
        public xRandNetDBContext() {
        }

        public xRandNetDBContext(DbContextOptions<xRandNetDBContext> options)
            : base(options) {
        }

        public virtual DbSet<App> App { get; set; }
        public virtual DbSet<Auth> Auth { get; set; }
        public virtual DbSet<Bugs> Bugs { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<Info> Info { get; set; }
        public virtual DbSet<Links> Links { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public override int SaveChanges() {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<App>();
            updateUpdatedProperty<Auth>();
            updateUpdatedProperty<Bugs>();
            updateUpdatedProperty<Files>();
            updateUpdatedProperty<Info>();
            updateUpdatedProperty<Links>();
            updateUpdatedProperty<News>();
            updateUpdatedProperty<Users>();

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo) {
                entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http: //go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql(
                    "server=localhost;port=3306;user=xrandnet;password=xRandNet5%;database=xrandnet",
                    x => x.ServerVersion("5.7.28-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<App>(entity => {
                entity.ToTable("app");

                entity.HasIndex(e => e.FileId)
                    .HasName("app_files_id_fk");

                entity.HasIndex(e => e.Version)
                    .HasName("version_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FileId)
                    .HasColumnName("fileId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnName("releaseDate")
                    .HasColumnType("date");

                entity.Property(e => e.ReleaseNotes)
                    .HasColumnName("releaseNotes")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.App)
                    .HasForeignKey(d => d.FileId)
                    .HasConstraintName("app_files_id_fk");
            });

            modelBuilder.Entity<Auth>(entity => {
                entity.HasNoKey();

                entity.ToTable("auth");

                entity.Property(e => e.JwtSecretKey)
                    .IsRequired()
                    .HasColumnName("jwtSecretKey")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Bugs>(entity => {
                entity.ToTable("bugs");

                entity.HasIndex(e => e.Version)
                    .HasName("bugs_app_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.ReportDate)
                    .HasColumnName("reportDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Reporter)
                    .IsRequired()
                    .HasColumnName("reporter")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Summary)
                    .IsRequired()
                    .HasColumnName("summary")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Files>(entity => {
                entity.ToTable("files");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Data).HasColumnName("data");

                entity.Property(e => e.DownloadId)
                    .HasColumnName("downloadId")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.MimeType)
                    .HasColumnName("mimeType")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasColumnType("varchar(31)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Info>(entity => {
                entity.ToTable("info");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Links>(entity => {
                entity.ToTable("links");

                entity.HasIndex(e => e.Id)
                    .HasName("links_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<News>(entity => {
                entity.ToTable("news");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.DatePosted)
                    .HasColumnName("datePosted")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Users>(entity => {
                entity.ToTable("users");

                entity.HasIndex(e => e.Username)
                    .HasName("UKr43af9ap4edm43mmtq01oddj6")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.FirstName)
                    .HasColumnName("firstName")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.IsAdmin)
                    .HasColumnName("isAdmin")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LastName)
                    .HasColumnName("lastName")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
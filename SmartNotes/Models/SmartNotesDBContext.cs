using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SmartNotes.Models
{
    public partial class SmartNotesDBContext : DbContext
    {
        public SmartNotesDBContext()
        {
        }

        public SmartNotesDBContext(DbContextOptions<SmartNotesDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Notes> Notes { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-BKAK5EE\\SQLEXPRESS;Initial Catalog=SmartNotesDB;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Images>(entity =>
            {
                entity.ToTable("images");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .HasMaxLength(100);

                entity.Property(e => e.Noteid).HasColumnName("noteid");

                entity.HasOne(d => d.Note)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.Noteid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_images_notes");
            });

            modelBuilder.Entity<Notes>(entity =>
            {
                entity.ToTable("notes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdat)
                    .HasColumnName("createdat")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NoteText)
                    .IsRequired()
                    .HasColumnName("note_text")
                    .HasMaxLength(1000);

                entity.Property(e => e.Pinned).HasColumnName("pinned");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(50);

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasMaxLength(50);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_notes_users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email)
                    .HasName("IX_users")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

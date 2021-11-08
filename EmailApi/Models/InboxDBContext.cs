using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EmailApi.Models
{
    public partial class InboxDBContext : DbContext
    {
        public InboxDBContext(DbContextOptions<InboxDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Inbox> Inboxes { get; set; }
        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Email>(entity =>
            {
                entity.ToTable("Email");

                entity.Property(e => e.EmailId).HasColumnName("Email_ID");

                entity.Property(e => e.Content)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LabelId).HasColumnName("Label_ID");

                entity.Property(e => e.Sender)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Label)
                    .WithMany(p => p.Emails)
                    .HasForeignKey(d => d.LabelId)
                    .HasConstraintName("FK__Email__Label_ID__2E1BDC42");

                entity.HasOne(d => d.SenderNavigation)
                    .WithMany(p => p.Emails)
                    .HasForeignKey(d => d.Sender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Email__Sender__2D27B809");
            });

            modelBuilder.Entity<Inbox>(entity =>
            {
                entity.HasKey(e => new { e.Username, e.EmailId })
                    .HasName("PK_UserEmail")
                    .IsClustered(false);

                entity.ToTable("Inbox");

                entity.Property(e => e.Username)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EmailId).HasColumnName("Email_ID");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Email)
                    .WithMany(p => p.Inboxes)
                    .HasForeignKey(d => d.EmailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inbox__Email_ID__31EC6D26");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Inboxes)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inbox__Username__30F848ED");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.ToTable("Label");

                entity.Property(e => e.LabelId).HasColumnName("Label_ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__User__536C85E582809185");

                entity.ToTable("User");

                entity.Property(e => e.Username)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

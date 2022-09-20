using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using GetFeedBack.Models;


namespace GetFeedBack.Models
{
    public partial class FeedbackContext : DbContext
    {
        public FeedbackContext()
        {
        }

        public FeedbackContext(DbContextOptions<FeedbackContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<FeedBackDetails> FeedBackDetails { get; set; }
        public virtual DbSet<FeedBacks> FeedBacks { get; set; }
        public virtual DbSet<Links> Links { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=MRWALL\\SQLEXPRESS;Database=Feedback;User Id=sa;Password=mrwall;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admins>(entity =>
            {
                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(20);
            });

            modelBuilder.Entity<FeedBackDetails>(entity =>
            {
                entity.Property(e => e.Advantage)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Disavantage)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FeedbackId).HasColumnName("Feedback_Id");

                entity.Property(e => e.Opinion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SenderName)
                    .HasColumnName("Sender_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Feedback)
                    .WithMany(p => p.FeedBackDetails)
                    .HasForeignKey(d => d.FeedbackId)
                    .HasConstraintName("FK_FeedBacks_TFeedBack");
            });

            modelBuilder.Entity<FeedBacks>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FeedBacks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_FeedBacks_Users");
            });

            modelBuilder.Entity<Links>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Deadline).HasColumnType("datetime");

                entity.Property(e => e.FeedbackId).HasColumnName("Feedback_Id");

                entity.Property(e => e.Link)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Feedback)
                    .WithMany(p => p.Links)
                    .HasForeignKey(d => d.FeedbackId)
                    .HasConstraintName("FK_Links_FeedBacks");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(20);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<GetFeedBack.Models.FeedbackUser> FeedbackUser { get; set; }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ApiBase.Models;

#nullable disable

namespace ApiBase.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnswersVr> AnswersVrs { get; set; }
        public virtual DbSet<QuestionsVr> QuestionsVrs { get; set; }
        
        public virtual DbSet<UsersVr> UsersVrs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=tcp:db-amstel.database.windows.net;database=Int-db-amstel;persist security info=True;user id=Adminamstel;password=A4#t3q@lt2;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AnswersVr>(entity =>
            {
                entity.ToTable("AnswersVR");

                entity.Property(e => e.AnswerDescription)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.AnswersVrs)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_AnswersVR_QuestionsVR");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AnswersVrs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AnswersVR_UsersVR");
            });

            modelBuilder.Entity<QuestionsVr>(entity =>
            {
                entity.ToTable("QuestionsVR");

                entity.Property(e => e.QuestionDescription)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<UsersVr>(entity =>
            {
                entity.ToTable("UsersVR");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VerificationCode)
                    //.IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.City)
                 .HasMaxLength(50)
                 .IsUnicode(false);

                entity.Property(e => e.Colony)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Province)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

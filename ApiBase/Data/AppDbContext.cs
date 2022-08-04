
//using Microsoft.EntityFrameworkCore;
//using ApiBase.Models;



//namespace ApiBase.Data
//{
//    public partial class AppDbContext : DbContext
//    {
//        public AppDbContext()
//        {
//        }

//        public AppDbContext(DbContextOptions<AppDbContext> options)
//            : base(options)
//        {
//        }

//        public virtual DbSet<Users> Users { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {

//                optionsBuilder.UseSqlServer("Server=(localdb)\\serverfirst;Database=ApiLogin;Integrated Security=true");
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Users>(entity =>
//            {
//                entity.ToTable("users");

//               entity.Property(e => e.LastName)
//                    .IsRequired()
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.Email)
//                    .IsRequired()
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.FirstName)
//                    .IsRequired()
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

              

//                entity.Property(e => e.Username)
//                    .IsRequired()
//                    .HasMaxLength(20)
//                    .IsUnicode(false);
//            });

//            OnModelCreatingPartial(modelBuilder);
//        }

//        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//    }
//}

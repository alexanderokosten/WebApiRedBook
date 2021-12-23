using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApiRedBook.Model
{
    public partial class RedBookBaseContext : DbContext
    {
        public RedBookBaseContext()
        {
        }

        public RedBookBaseContext(DbContextOptions<RedBookBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClassItem> ClassItem { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Kingdom> Kingdom { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Type> Type { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-EA2TOMB;Database=RedBookBase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassItem>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TypeId).HasColumnName("Type_Id");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.ClassItem)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassItem_Type");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.Biology).IsRequired();

                entity.Property(e => e.ClassItemId).HasColumnName("ClassItem_Id");

                entity.Property(e => e.Image).IsRequired();

                entity.Property(e => e.LimitingFactors).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.SecurityMeasures).IsRequired();

                entity.Property(e => e.Spread).IsRequired();

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.HasOne(d => d.ClassItem)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.ClassItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Item_ClassItem");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Item_Status");
            });

            modelBuilder.Entity<Kingdom>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.Property(e => e.KingdomId).HasColumnName("Kingdom_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(d => d.Kingdom)
                    .WithMany(p => p.Type)
                    .HasForeignKey(d => d.KingdomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Type_Kingdom");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

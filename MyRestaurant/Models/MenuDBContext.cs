using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyRestaurant.Models
{
    public partial class MenuDBContext : DbContext
    {
        public MenuDBContext()
        {
        }

        public MenuDBContext(DbContextOptions<MenuDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<CategoryDish> CategoryDishes { get; set; } = null!;
        public virtual DbSet<Dish> Dishes { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<MenuCategory> MenuCategories { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=M5387486\\SQLEXPRESS;Database=MenuDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryDescription).IsUnicode(false);

                entity.Property(e => e.CategoryImage)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CategroyName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CategoryDish>(entity =>
            {
                entity.ToTable("CategoryDish");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryDishes)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__CategoryD__Categ__5812160E");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.CategoryDishes)
                    .HasForeignKey(d => d.DishId)
                    .HasConstraintName("FK__CategoryD__DishI__59063A47");
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dish");

                entity.Property(e => e.DishDescription)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.DishImage)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.DishName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DishNature)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.MenuDescription).IsUnicode(false);

                entity.Property(e => e.MenuImage)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MenuCategory>(entity =>
            {
                entity.ToTable("MenuCategory");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.MenuCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__MenuCateg__Categ__5070F446");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MenuCategories)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK__MenuCateg__MenuI__4F7CD00D");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

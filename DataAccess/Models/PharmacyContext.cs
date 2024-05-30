using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess.Models
{
    public partial class PharmacyContext : DbContext
    {
        public PharmacyContext()
        {
        }

        public PharmacyContext(DbContextOptions<PharmacyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Description> Descriptions { get; set; } = null!;
        public virtual DbSet<DescriptionMedicine> DescriptionMedicines { get; set; } = null!;
        public virtual DbSet<Factory> Factories { get; set; } = null!;
        public virtual DbSet<Ingredient> Ingredients { get; set; } = null!;
        public virtual DbSet<Medicine> Medicines { get; set; } = null!;
        public virtual DbSet<MedicineIngredient> MedicineIngredients { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=Pharmacy;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Description>(entity =>
            {
                entity.ToTable("description");

                entity.Property(e => e.Description1)
                    .HasMaxLength(50)
                    .HasColumnName("Description");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Descriptions)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientMedicine_Patient");
            });

            modelBuilder.Entity<DescriptionMedicine>(entity =>
            {
                entity.ToTable("DescriptionMedicine");

                entity.HasIndex(e => new { e.DescriptionId, e.MedicineId }, "IX_DescriptionMedicine")
                    .IsUnique();

                entity.Property(e => e.Count).HasColumnName("count");

                entity.HasOne(d => d.Description)
                    .WithMany(p => p.DescriptionMedicines)
                    .HasForeignKey(d => d.DescriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DescriptionMedicine_description");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.DescriptionMedicines)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DescriptionMedicine_Medicine");
            });

            modelBuilder.Entity<Factory>(entity =>
            {
                entity.ToTable("Factory");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("Ingredient");

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.ToTable("Medicine");

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.Dose).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.TradeName).HasMaxLength(50);

                entity.HasOne(d => d.ActiveSubstance)
                    .WithMany(p => p.Medicines)
                    .HasForeignKey(d => d.ActiveSubstanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Medicine_Ingredient");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Medicines)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Medicine_Category");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.Medicines)
                    .HasForeignKey(d => d.FactoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Medicine_Factory");
            });

            modelBuilder.Entity<MedicineIngredient>(entity =>
            {
                entity.ToTable("MedicineIngredient");

                entity.HasIndex(e => new { e.IngredientId, e.MedicineId }, "IX_MedicineIngredient")
                    .IsUnique();

                entity.Property(e => e.Ratio).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.MedicineIngredients)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MedicineIngredient_Ingredient");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.MedicineIngredients)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MedicineIngredient_Medicine");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");

                entity.Property(e => e.Address)
                    .HasMaxLength(150)
                    .HasColumnName("address");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasColumnType("numeric(18, 0)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

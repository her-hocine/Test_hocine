using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Protocols;

#nullable disable

namespace Database_First.Models
{
    public partial class bdgplccContext : DbContext
    {
        public bdgplccContext() 
        {
        }

        public bdgplccContext(DbContextOptions<bdgplccContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Emprunter> Emprunters { get; set; }
        public virtual DbSet<Livre> Livres { get; set; }
        public virtual DbSet<Membre> Membres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=bdgplcc;Trusted_Connection=True;");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "French_CI_AS");

            modelBuilder.Entity<Emprunter>(entity =>
            {
                entity.HasKey(e => e.IdE);

                entity.Property(e => e.DateEmp)
                    .HasColumnType("date")
                    .HasColumnName("dateEmp");

                entity.Property(e => e.DateRetour)
                    .HasColumnType("date")
                    .HasColumnName("dateRetour");

                entity.HasOne(d => d.IdLNavigation)
                    .WithMany(p => p.Emprunters)
                    .HasForeignKey(d => d.IdL)
                    .HasConstraintName("FK_Emprunters_Livres1");

                entity.HasOne(d => d.IdMNavigation)
                    .WithMany(p => p.Emprunters)
                    .HasForeignKey(d => d.IdM)
                    .HasConstraintName("FK_Emprunters_Membres1");
            });

            modelBuilder.Entity<Livre>(entity =>
            {
                entity.HasKey(e => e.IdL);

                entity.Property(e => e.Auteur)
                    .HasMaxLength(50)
                    .HasColumnName("auteur");

                entity.Property(e => e.Categories)
                    .HasMaxLength(50) 
                    .HasColumnName("categories");

                entity.Property(e => e.NbExemlpaire).HasColumnName("NbExemlpaire");

                entity.Property(e => e.NbExemlpaireTotal).HasColumnName("NbExemlpaireTotal");

                entity.Property(e => e.Titre)
                    .HasMaxLength(50)
                    .HasColumnName("titre");
            });

            modelBuilder.Entity<Membre>(entity =>
            {
                entity.HasKey(e => e.IdM);

                entity.Property(e => e.Courriel)
                    .HasMaxLength(50)
                    .HasColumnName("courriel");

                entity.Property(e => e.MotPasse)
                    .HasMaxLength(50)
                    .HasColumnName("MotPasse");

                entity.Property(e => e.Nom)
                    .HasMaxLength(10)
                    .HasColumnName("nom")
                    .IsFixedLength(true);

                entity.Property(e => e.Prenom)
                    .HasMaxLength(50)
                    .HasColumnName("prenom");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

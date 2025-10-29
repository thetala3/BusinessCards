using BusinessCards.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCards.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<BusinessCard> BusinessCards => Set<BusinessCard>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BusinessCard>(entity =>
            {
                entity.ToTable("BusinessCards");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Gender).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Email).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Phone).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Address).HasMaxLength(500);
                entity.Property(x => x.DateOfBirth).HasColumnType("date");
                entity.Property(x => x.Photo).HasColumnType("nvarchar(max)");
                entity.HasIndex(x => x.Name);
                entity.HasIndex(x => x.Email);
                entity.HasIndex(x => x.Phone);
                entity.HasIndex(x => x.Gender);
                entity.HasIndex(x => x.DateOfBirth);

            });
        }
    }
}

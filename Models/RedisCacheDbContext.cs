using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RedisCacheExample.Models;

public partial class RedisCacheDbContext : DbContext
{
    public RedisCacheDbContext()
    {
    }

    public RedisCacheDbContext(DbContextOptions<RedisCacheDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblEmployee> TblEmployees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-53UNUI6G;Initial Catalog=RedisCacheDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_Empl__3214EC071A8992A7");

            entity.ToTable("tbl_Employee");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnName("Created_date");
            entity.Property(e => e.Designation).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.EmpName)
                .HasMaxLength(50)
                .HasColumnName("Emp_Name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

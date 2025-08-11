using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StudentApi.DbFirst.Entities;

namespace StudentApi.DbFirst;

public partial class StudentDbFirstContext : DbContext
{
    public StudentDbFirstContext()
    {
    }

    public StudentDbFirstContext(DbContextOptions<StudentDbFirstContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=np:\\\\.\\pipe\\LOCALDB#819C9B24\\tsql\\query;Database=Lab1Db;Trusted_Connection=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Students_Email").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

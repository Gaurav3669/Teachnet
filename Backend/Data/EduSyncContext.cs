using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SampleProject.Models;

namespace SampleProject.Data;

public partial class EduSyncContext : DbContext
{
    public EduSyncContext()
    {
    }

    public EduSyncContext(DbContextOptions<EduSyncContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assessment> Assessments { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DIPESH-PC;Initial Catalog=EduSyncDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.ToTable("Assessment");

            entity.Property(e => e.AssessmentId).ValueGeneratedNever();
            entity.Property(e => e.Questions)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");

            entity.Property(e => e.CourseId).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MediaUrl)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.ToTable("Result");

            entity.Property(e => e.ResultId).ValueGeneratedNever();
            entity.Property(e => e.AttemptDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

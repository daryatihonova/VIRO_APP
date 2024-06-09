using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VIRO_APP.Models;

public partial class ViroContext : DbContext
{
    public ViroContext()
    {
    }

    public ViroContext(DbContextOptions<ViroContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog = Viro; Integrated Security = True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("Course_ID");
            entity.Property(e => e.AudiencyCategory)
                .HasMaxLength(100)
                .HasColumnName("Audiency_Category");
            entity.Property(e => e.CourseSupervisor)
                .HasMaxLength(50)
                .HasColumnName("Course_Supervisor");
            entity.Property(e => e.FinishDate)
                .HasColumnType("date")
                .HasColumnName("Finish_Date");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NumberOfHourse).HasColumnName("Number_Of_Hourse");
            entity.Property(e => e.NumberOfPeople).HasColumnName("Number_Of_People");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("Start_Date");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.StudentId).HasColumnName("Student_ID");
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.Education).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("Last_Name");
            entity.Property(e => e.Login).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(10);
            entity.Property(e => e.Patronymic).HasMaxLength(50);
            entity.Property(e => e.Post).HasMaxLength(50);
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.ToTable("Student_Course");

            entity.Property(e => e.StudentCourseId).HasColumnName("Student_Course_ID");
            entity.Property(e => e.CourseId).HasColumnName("Course_ID");
            entity.Property(e => e.StudentId).HasColumnName("Student_ID");

            entity.HasOne(d => d.Course).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Student_Course_Course");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Student_Course_Student");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

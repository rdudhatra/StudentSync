using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StudentSync.Data.Models;

namespace StudentSync.Data.Data;

public partial class StudentSyncDbContext : DbContext
{
    public StudentSyncDbContext()
    {
    }

    public StudentSyncDbContext(DbContextOptions<StudentSyncDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Batch> Batches { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseExam> CourseExams { get; set; }

    public virtual DbSet<CourseFee> CourseFees { get; set; }

    public virtual DbSet<CourseSyllabus> CourseSyllabi { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Inquiry> Inquiries { get; set; }

    public virtual DbSet<InquiryFollowUp> InquiryFollowUps { get; set; }

    public virtual DbSet<StudentAssessment> StudentAssessments { get; set; }

    public virtual DbSet<StudentAttendance> StudentAttendances { get; set; }

    public virtual DbSet<StudentInstallment> StudentInstallments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-KIC018L;Initial Catalog=StudentSyncDb;User ID=sa;Password=saadmin;TrustServerCertificate=True");

    
}

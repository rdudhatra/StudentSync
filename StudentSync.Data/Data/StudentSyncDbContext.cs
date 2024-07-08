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

    //Enrollments Crud Operation 
    public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
    {
        return await Enrollments.FromSqlRaw("EXEC GetAllEnrollments").ToListAsync();
    }

    public async Task<Enrollment> GetEnrollmentByIdAsync(string enrollmentNo)
    {
        return await Enrollments.FromSqlRaw("EXEC GetEnrollmentById @p0", enrollmentNo).FirstOrDefaultAsync();
    }

    public async Task CreateEnrollmentAsync(Enrollment enrollment)
    {
        await Database.ExecuteSqlRawAsync("EXEC CreateEnrollment @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9",
            enrollment.EnrollmentNo, enrollment.EnrollmentDate, enrollment.BatchId, enrollment.CourseId,
            enrollment.CourseFeeId, enrollment.InquiryNo, enrollment.IsActive, enrollment.Remarks,
            enrollment.CreatedBy, enrollment.CreatedDate);
    }

    public async Task UpdateEnrollmentAsync(Enrollment enrollment)
    {
        await Database.ExecuteSqlRawAsync("EXEC UpdateEnrollment @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9",
            enrollment.EnrollmentNo, enrollment.EnrollmentDate, enrollment.BatchId, enrollment.CourseId,
            enrollment.CourseFeeId, enrollment.InquiryNo, enrollment.IsActive, enrollment.Remarks,
            enrollment.UpdatedBy, enrollment.UpdatedDate);
    }

    public async Task DeleteEnrollmentAsync(string enrollmentNo)
    {
        await Database.ExecuteSqlRawAsync("EXEC DeleteEnrollment @p0", enrollmentNo);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Data Source=DESKTOP-KIC018L;Initial Catalog=StudentSyncDb;User ID=sa;Password=saadmin;TrustServerCertificate=True");

    
}

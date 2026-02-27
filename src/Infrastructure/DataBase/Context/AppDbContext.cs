using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options) { }

    public virtual DbSet<Classroom> Classrooms { get; set; }
    public virtual DbSet<Curator> Curators { get; set; }
    public virtual DbSet<Employer> Employers { get; set; }
    public virtual DbSet<Exam> Exams { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<Okr> Okrs { get; set; }
    public virtual DbSet<Removal> Removals { get; set; }
    public virtual DbSet<Schedule> Schedules { get; set; }
    public virtual DbSet<Specialty> Specialties { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<SubjectTeacher> SubjectTeachers { get; set; }
    public virtual DbSet<SubjectTeacherSchedule> SubjectTeacherSchedules { get; set; }
    public virtual DbSet<SubjectTeacherSemester> SubjectTeacherSemesters { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
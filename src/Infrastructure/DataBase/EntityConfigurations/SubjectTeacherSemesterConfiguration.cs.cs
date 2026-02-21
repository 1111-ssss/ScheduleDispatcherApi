using Infrastructure.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class SubjectTeacherSemesterConfiguration : IEntityTypeConfiguration<SubjectTeacherSemester>
{
    public void Configure(EntityTypeBuilder<SubjectTeacherSemester> entity)
    {
        entity.HasKey(e => e.IdsubjectTeacherSemester).HasName("subject_teacher_semester_pkey");

        entity.ToTable("subject_teacher_semester");

        entity.Property(e => e.IdsubjectTeacherSemester)
            .UseIdentityAlwaysColumn()
            .HasColumnName("idsubject_teacher_semester");
        entity.Property(e => e.IdsubjectTeacher).HasColumnName("idsubject_teacher");
        entity.Property(e => e.Semester1).HasColumnName("semester1");
        entity.Property(e => e.Semester2).HasColumnName("semester2");

        entity.HasOne(d => d.IdsubjectTeacherNavigation).WithMany(p => p.Subjectteachersemesters)
            .HasForeignKey(d => d.IdsubjectTeacher)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_stsem_subj_teacher");
    }
}
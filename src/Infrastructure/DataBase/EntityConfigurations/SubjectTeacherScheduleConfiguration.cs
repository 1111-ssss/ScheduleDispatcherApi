using Infrastructure.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class SubjectTeacherScheduleConfiguration : IEntityTypeConfiguration<SubjectTeacherSchedule>
{
    public void Configure(EntityTypeBuilder<SubjectTeacherSchedule> entity)
    {
        entity.HasKey(e => new { e.IdsubjectTeacher, e.IdSchedule }).HasName("subject_teacher_schedule_pkey");

        entity.ToTable("subject_teacher_schedule");

        entity.Property(e => e.IdsubjectTeacher).HasColumnName("idsubject_teacher");
        entity.Property(e => e.IdSchedule).HasColumnName("id_schedule");
        entity.Property(e => e.ExaminationHours).HasColumnName("examination_hours");
        entity.Property(e => e.GroupSplit).HasColumnName("group_split");
        entity.Property(e => e.LectureHours1term).HasColumnName("lecture_hours_1term");
        entity.Property(e => e.LectureHours2term).HasColumnName("lecture_hours_2term");
        entity.Property(e => e.PracticalHours1term).HasColumnName("practical_hours_1term");
        entity.Property(e => e.PracticalHours2term).HasColumnName("practical_hours_2term");

        entity.HasOne(d => d.IdScheduleNavigation).WithMany(p => p.SubjectTeacherSchedules)
            .HasForeignKey(d => d.IdSchedule)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_sts_schedule");

        entity.HasOne(d => d.IdsubjectTeacherNavigation).WithMany(p => p.SubjectTeacherSchedules)
            .HasForeignKey(d => d.IdsubjectTeacher)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_sts_subj_teacher");
    }
}
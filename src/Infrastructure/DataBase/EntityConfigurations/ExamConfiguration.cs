using Infrastructure.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> entity)
    {
        entity.HasKey(e => e.IdSchedule).HasName("exam_pkey");

        entity.ToTable("exam");

        entity.Property(e => e.IdSchedule)
            .ValueGeneratedNever()
            .HasColumnName("id_schedule");

        entity.HasOne(d => d.IdScheduleNavigation).WithOne(p => p.Exam)
            .HasForeignKey<Exam>(d => d.IdSchedule)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_exam_schedule");
    }
}
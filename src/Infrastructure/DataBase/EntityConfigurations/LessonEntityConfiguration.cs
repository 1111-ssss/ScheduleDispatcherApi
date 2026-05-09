using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;


public class LessonConfiguration : IEntityTypeConfiguration<LessonEntity>
{
    public void Configure(EntityTypeBuilder<LessonEntity> builder)
    {
        builder.ToTable("lessons");

        builder.HasKey(x => new { x.Groupid, x.LessonNumber });

        builder.Property(x => x.Groupid).HasColumnName("groupid");
        builder.Property(x => x.Subject1).HasColumnName("subject_1").IsRequired();
        builder.Property(x => x.Subject2).HasColumnName("subject_2");
        builder.Property(x => x.Teacher1).HasColumnName("teacher_1").IsRequired();
        builder.Property(x => x.Teacher2).HasColumnName("teacher_2");
        builder.Property(x => x.Classroom1).HasColumnName("classroom_1").IsRequired();
        builder.Property(x => x.Classroom2).HasColumnName("classroom_2");
        builder.Property(x => x.StartTime).HasColumnName("starttime").IsRequired();
        builder.Property(x => x.EndTime).HasColumnName("endtime").IsRequired();
        builder.Property(x => x.LessonNumber).HasColumnName("LessonNumber").IsRequired();
        builder.Property(x => x.Split).HasColumnName("split").IsRequired();
    }
}

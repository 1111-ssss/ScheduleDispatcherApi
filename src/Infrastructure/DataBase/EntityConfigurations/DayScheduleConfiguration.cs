using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class DayScheduleConfiguration : IEntityTypeConfiguration<DayScheduleEntity>
{
    public void Configure(EntityTypeBuilder<DayScheduleEntity> builder)
    {
        builder.ToTable("day_schedule");
        builder.Property(x => x.Id)
    .ValueGeneratedOnAdd()
    .UseIdentityAlwaysColumn();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.GroupName).HasColumnName("group_name").IsRequired();
        builder.Property(x => x.Date).HasColumnName("date").IsRequired();

        builder.HasMany(x => x.Lessons)
               .WithOne(x => x.daySchedule)
               .HasForeignKey(x => x.Groupid)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

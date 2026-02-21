using Infrastructure.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> entity)
    {
        entity.HasKey(e => e.IdSchedule).HasName("schedule_pkey");

        entity.ToTable("schedule");

        entity.Property(e => e.IdSchedule)
            .UseIdentityAlwaysColumn()
            .HasColumnName("id_schedule");
        entity.Property(e => e.Date1)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("date_1");
        entity.Property(e => e.Date2)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("date_2");
        entity.Property(e => e.IsOver).HasColumnName("is_over");
        entity.Property(e => e.IsPractical).HasColumnName("is_practical");
        entity.Property(e => e.Lessonnumber).HasColumnName("lessonnumber");
    }
}
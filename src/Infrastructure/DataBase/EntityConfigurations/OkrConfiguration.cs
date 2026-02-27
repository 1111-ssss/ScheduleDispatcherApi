using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class OkrConfiguration : IEntityTypeConfiguration<Okr>
{
    public void Configure(EntityTypeBuilder<Okr> entity)
    {
        entity.HasKey(e => e.IdSchedule).HasName("okr_pkey");

        entity.ToTable("okr");

        entity.Property(e => e.IdSchedule)
            .ValueGeneratedNever()
            .HasColumnName("id_schedule");

        entity.HasOne(d => d.IdScheduleNavigation).WithOne(p => p.Okr)
            .HasForeignKey<Okr>(d => d.IdSchedule)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_okr_schedule");
    }
}
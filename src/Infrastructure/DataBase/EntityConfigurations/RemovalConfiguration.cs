using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class RemovalConfiguration : IEntityTypeConfiguration<Removal>
{
    public void Configure(EntityTypeBuilder<Removal> entity)
    {
        entity.HasKey(e => e.IdRemoval).HasName("removal_pkey");

        entity.ToTable("removal");

        entity.Property(e => e.IdRemoval)
            .UseIdentityAlwaysColumn()
            .HasColumnName("id_removal");
        entity.Property(e => e.IdSchedule1).HasColumnName("id_schedule1");
        entity.Property(e => e.IdSchedule2).HasColumnName("id_schedule2");

        entity.HasOne(d => d.IdSchedule1Navigation).WithMany(p => p.RemovalIdSchedule1Navigations)
            .HasForeignKey(d => d.IdSchedule1)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_removal_sched1");

        entity.HasOne(d => d.IdSchedule2Navigation).WithMany(p => p.RemovalIdSchedule2Navigations)
            .HasForeignKey(d => d.IdSchedule2)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_removal_sched2");
    }
}
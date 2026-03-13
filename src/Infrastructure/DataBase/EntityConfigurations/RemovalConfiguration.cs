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

        entity.HasIndex(e => e.IdSchedule1).IsUnique();
        entity.HasIndex(e => e.IdSchedule2).IsUnique();

        entity.Property(e => e.IdRemoval).UseIdentityAlwaysColumn().HasColumnName("id_removal");

        entity.HasOne(d => d.IdSchedule1Navigation)
            .WithOne(p => p.RemovalAsFirst)
            .HasForeignKey<Removal>(d => d.IdSchedule1)
            .OnDelete(DeleteBehavior.ClientSetNull);

        entity.HasOne(d => d.IdSchedule2Navigation)
            .WithOne(p => p.RemovalAsSecond)
            .HasForeignKey<Removal>(d => d.IdSchedule2)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
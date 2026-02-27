using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class CuratorConfiguration : IEntityTypeConfiguration<Curator>
{
    public void Configure(EntityTypeBuilder<Curator> entity)
    {
        entity.HasKey(e => e.IdCurator).HasName("curator_pkey");

        entity.ToTable("curator");

        entity.Property(e => e.IdCurator)
            .UseIdentityAlwaysColumn()
            .HasColumnName("id_curator");
        entity.Property(e => e.CnG)
            .HasMaxLength(4)
            .HasColumnName("cn_g");
        entity.Property(e => e.CnT)
            .HasMaxLength(15)
            .HasColumnName("cn_t");

        entity.HasOne(d => d.CnGNavigation).WithMany(p => p.Curators)
            .HasForeignKey(d => d.CnG)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_curator_group");

        entity.HasOne(d => d.CnTNavigation).WithMany(p => p.Curators)
            .HasForeignKey(d => d.CnT)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_curator_teacher");
    }
}
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> entity)
    {
        entity.HasKey(e => e.CnG).HasName("group_pkey");

        entity.ToTable("group");

        entity.Property(e => e.CnG)
            .HasMaxLength(4)
            .HasColumnName("cn_g");
        entity.Property(e => e.CnSpec)
            .HasMaxLength(30)
            .HasColumnName("cn_spec");
        entity.Property(e => e.Cours).HasColumnName("cours");
        entity.Property(e => e.Name).HasMaxLength(4);

        entity.HasOne(d => d.CnSpecNavigation).WithMany(p => p.Groups)
            .HasForeignKey(d => d.CnSpec)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_group_specialty");
    }
}
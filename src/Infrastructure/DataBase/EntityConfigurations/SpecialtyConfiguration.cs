using Infrastructure.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> entity)
    {
        entity.HasKey(e => e.CnSpec).HasName("specialty_pkey");

        entity.ToTable("specialty");

        entity.Property(e => e.CnSpec)
            .HasMaxLength(30)
            .HasColumnName("cn_spec");
        entity.Property(e => e.CnT)
            .HasMaxLength(15)
            .HasColumnName("cn_t");
        entity.Property(e => e.Department)
            .HasMaxLength(200)
            .HasColumnName("department");
        entity.Property(e => e.Educationperiod).HasColumnName("educationperiod");
        entity.Property(e => e.Fullname)
            .HasMaxLength(100)
            .HasColumnName("fullname");
        entity.Property(e => e.Name).HasMaxLength(20);

        entity.HasOne(d => d.CnTNavigation).WithMany(p => p.Specialties)
            .HasForeignKey(d => d.CnT)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_specialty_teacher");
    }
}
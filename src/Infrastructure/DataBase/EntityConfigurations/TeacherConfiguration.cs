using Infrastructure.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> entity)
    {
        entity.HasKey(e => e.CnT).HasName("teacher_pkey");

        entity.ToTable("teacher");

        entity.Property(e => e.CnT)
            .HasMaxLength(15)
            .HasColumnName("cn_t");
        entity.Property(e => e.CnE)
            .HasMaxLength(15)
            .HasColumnName("cn_e");
        entity.Property(e => e.Idcategory).HasColumnName("idcategory");
        entity.Property(e => e.Idck).HasColumnName("idck");

        entity.HasOne(d => d.CnENavigation).WithMany(p => p.Teachers)
            .HasForeignKey(d => d.CnE)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_teacher_employer");
    }
}
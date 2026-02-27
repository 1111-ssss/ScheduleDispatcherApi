using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
{
    public void Configure(EntityTypeBuilder<Employer> entity)
    {
        entity.HasKey(e => e.CnE).HasName("employer_pkey");

        entity.ToTable("employer");

        entity.Property(e => e.CnE)
            .HasMaxLength(15)
            .HasColumnName("cn_e");
        entity.Property(e => e.FatherName)
            .HasMaxLength(35)
            .HasColumnName("father_name");
        entity.Property(e => e.Name)
            .HasMaxLength(35)
            .HasColumnName("name");
        entity.Property(e => e.Surname)
            .HasMaxLength(35)
            .HasColumnName("surname");
    }
}
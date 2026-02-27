using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> entity)
    {
        entity.HasKey(e => e.IdClassroom).HasName("classrooms_pkey");

        entity.ToTable("classrooms");

        entity.Property(e => e.IdClassroom)
            .UseIdentityAlwaysColumn()
            .HasColumnName("id_classroom");

        entity.Property(e => e.ClassroomNumber)
            .HasColumnName("classroom_number");

        entity.Property(e => e.IsPcClassroom)
            .HasColumnName("is_pc_classroom");
    }
}
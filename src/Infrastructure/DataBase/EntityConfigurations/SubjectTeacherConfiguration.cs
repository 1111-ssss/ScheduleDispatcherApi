using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class SubjectTeacherConfiguration : IEntityTypeConfiguration<SubjectTeacher>
{
    public void Configure(EntityTypeBuilder<SubjectTeacher> entity)
    {
        entity.HasKey(e => e.IdsubjectTeacher).HasName("subject_teacher_pkey");

        entity.ToTable("subject_teacher");

        entity.Property(e => e.IdsubjectTeacher)
            .UseIdentityAlwaysColumn()
            .HasColumnName("idsubject_teacher");
        entity.Property(e => e.CnG)
            .HasMaxLength(4)
            .HasColumnName("cn_g");
        entity.Property(e => e.CnT)
            .HasMaxLength(15)
            .HasColumnName("cn_t");
        entity.Property(e => e.IdSubject).HasColumnName("id_subject");

        entity.HasOne(d => d.CnGNavigation).WithMany(p => p.SubjectTeachers)
            .HasForeignKey(d => d.CnG)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_subj_teacher_group");

        entity.HasOne(d => d.CnTNavigation).WithMany(p => p.SubjectTeachers)
            .HasForeignKey(d => d.CnT)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_subj_teacher_teacher");

        entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.SubjectTeachers)
            .HasForeignKey(d => d.IdSubject)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_subj_teacher_subject");
    }
}
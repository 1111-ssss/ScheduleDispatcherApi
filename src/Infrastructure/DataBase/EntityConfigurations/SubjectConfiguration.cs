using Infrastructure.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> entity)
    {
        entity.HasKey(e => e.IdSubject).HasName("subject_pkey");

        entity.ToTable("subject");

        entity.Property(e => e.IdSubject)
            .UseIdentityAlwaysColumn()
            .HasColumnName("id_subject");
        entity.Property(e => e.CnSpec)
            .HasMaxLength(30)
            .HasColumnName("cn_spec");
        entity.Property(e => e.Fullname)
            .HasMaxLength(100)
            .HasColumnName("fullname");
        entity.Property(e => e.Name).HasMaxLength(50);
        entity.Property(e => e.Optional).HasColumnName("optional");
        entity.Property(e => e.PcClassNeed).HasColumnName("pc_class_need");
        entity.Property(e => e.Practichourcount).HasColumnName("practichourcount");
        entity.Property(e => e.Totalhourcount).HasColumnName("totalhourcount");

        entity.HasOne(d => d.CnSpecNavigation).WithMany(p => p.Subjects)
            .HasForeignKey(d => d.CnSpec)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_subject_specialty");

        entity.HasMany(d => d.IdClassrooms).WithMany(p => p.IdSubjects)
            .UsingEntity<Dictionary<string, object>>(
                "LimitationOnChoiceOfAudienceSubject",
                r => r.HasOne<Classroom>().WithMany()
                    .HasForeignKey("IdClassroom")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_lim_classroom"),
                l => l.HasOne<Subject>().WithMany()
                    .HasForeignKey("IdSubject")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_lim_subject"),
                j =>
                {
                    j.HasKey("IdSubject", "IdClassroom").HasName("limitation_on_choice_of_audience_subject_pkey");
                    j.ToTable("limitation_on_choice_of_audience_subject");
                    j.IndexerProperty<int>("IdSubject").HasColumnName("id_subject");
                    j.IndexerProperty<int>("IdClassroom").HasColumnName("id_classroom");
                });
    }
}
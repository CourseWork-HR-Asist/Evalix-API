using Domain.Evaluations;
using Domain.Resumes;
using Domain.Statuses;
using Domain.Vacancies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EvaluationConfigurator: IEntityTypeConfiguration<Evaluation>
{
    public void Configure(EntityTypeBuilder<Evaluation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new EvaluationId(x));
        
        builder.Property(x => x.Comment).HasColumnType("text");
        builder.Property(x => x.Score).HasColumnType("varchar(255)");
        
        builder.Property(x => x.VacancyId).HasConversion(x => x.Value, x => new VacancyId(x));
        builder.HasOne(x => x.Vacancy)
            .WithMany()
            .HasForeignKey(x => x.VacancyId)
            .HasConstraintName("fk_applications_vacancy_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.ResumeId).HasConversion(x => x.Value, x => new ResumeId(x));
        builder.HasOne(x => x.Resume)
            .WithMany()
            .HasForeignKey(x => x.ResumeId)
            .HasConstraintName("fk_applications_resume_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.StatusId).HasConversion(x => x.Value, x => new StatusId(x));
        builder.HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .HasConstraintName("fk_applications_status_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
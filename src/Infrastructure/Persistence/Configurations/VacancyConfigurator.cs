using Domain.Users;
using Domain.Vacancies;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class VacancyConfigurator: IEntityTypeConfiguration<Vacancy>
{
    public void Configure(EntityTypeBuilder<Vacancy> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new VacancyId(x));
        
        builder.Property(x => x.Title).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.Description).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.Experience).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.Education).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.RecruiterId).HasConversion(x => x.Value, x => new UserId(x));
        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        
        builder.HasOne(x => x.Recruiter)
            .WithMany()
            .HasForeignKey(x => x.RecruiterId)
            .HasConstraintName("fk_vacancies_users_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.VacancySkills)
            .WithOne(x => x.Vacancy)
            .HasForeignKey(x => x.VacancyId)
            .HasConstraintName("fk_vacancy_skills_vacancy_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Evaluations)
            .WithOne(x => x.Vacancy)
            .HasForeignKey(x => x.VacancyId)
            .HasConstraintName("fk_applications_vacancy_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
using Domain.Skills;
using Domain.Vacancies;
using Domain.VacancySkills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class VacancySkillConfigurator: IEntityTypeConfiguration<VacancySkill>
{
    public void Configure(EntityTypeBuilder<VacancySkill> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new VacancySkillId(x));
        
        builder.HasOne(x => x.Vacancy)
            .WithMany()
            .HasForeignKey(x => x.VacancyId)
            .HasConstraintName("fk_vacancy_skills_vacancy_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Skill)
            .WithMany()
            .HasForeignKey(x => x.SkillId)
            .HasConstraintName("fk_vacancy_skills_skill_id")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Experience).IsRequired().HasColumnType("varchar(25)");
        builder.Property(x => x.Level).IsRequired().HasColumnType("varchar(25)");
    }
}
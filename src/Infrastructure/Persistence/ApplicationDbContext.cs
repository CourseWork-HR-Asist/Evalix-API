using System.Reflection;
using Domain.Evaluations;
using Domain.Resumes;
using Domain.Roles;
using Domain.Skills;
using Domain.Statuses;
using Domain.Users;
using Domain.Vacancies;
using Domain.VacancySkills;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<VacancySkill> VacancySkills { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<Evaluation> Evaluations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
﻿using Domain.Roles;
using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfigurator: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new UserId(x));
        
        builder.Property(x => x.FirstName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.Email).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.PasswordHash).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");

        builder.Property(x => x.RoleId).HasConversion(x => x.Value, x => new RoleId(x));
        builder.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .HasConstraintName("fk_users_roles_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Resumes)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_resumes_users_id")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.Vacancies)
            .WithOne(x => x.Recruiter)
            .HasForeignKey(x => x.RecruiterId)
            .HasPrincipalKey(x => x.Id)
            .HasConstraintName("fk_vacancies_users_id")
            .OnDelete(DeleteBehavior.Cascade);

    }
}